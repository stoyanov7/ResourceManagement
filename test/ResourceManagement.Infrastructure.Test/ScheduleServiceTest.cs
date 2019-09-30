namespace ResourceManagement.Infrastructure.Test
{
    using Moq;
    using ResourceManagement.Domain.Logger;
    using ResourceManagement.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class ScheduleServiceTest
    {
        private readonly Schedule MultipleItemSchedule = new Schedule()
        {
            ScheduleItems = new List<ScheduleItem>
            {
                new ScheduleItem
                {
                    StartDateTime=new DateTime(2019, 5, 17, 15, 0, 0),
                    EndDateTime = new DateTime(2019, 5, 17, 16, 0, 0)
                },
                new ScheduleItem {
                    StartDateTime=new DateTime(2019, 5, 18, 17, 0, 0),
                    EndDateTime = new DateTime(2019, 5, 18, 18, 0, 0)
                }

            }
        };

        private readonly Schedule MyMultipleRecurringSchedule = new Schedule
        {
            RecurringSchedules = new List<RecurringSchedule>
            {
                    new RecurringSchedule
                    {
                        MinStartDateTime = new DateTime(2018, 6, 1),
                        MaxEndDateTime = new DateTime(2018, 6, 30),
                        CronPattern = "0 15 * * 1",
                        Duration = TimeSpan.FromHours(1)
                    },
                    new RecurringSchedule
                    {
                        MinStartDateTime = new DateTime(2020, 5, 1),
                        MaxEndDateTime = new DateTime(2020, 5, 31),
                        CronPattern = "0 15 * * 1",
                        Duration = TimeSpan.FromHours(1)
                    }
            }
        };

        private readonly Mock<IMyLogger<ScheduleService>> mockLogger;

        public ScheduleServiceTest()
        {
            this.mockLogger = new Mock<IMyLogger<ScheduleService>>();
        }

        [Fact]
        public void ShouldCreateTheScheduleItemsSpecified()
        {
            // Arrange
            var mySchedule = new Schedule()
            {
                ScheduleItems = new List<ScheduleItem>
                {
                    new ScheduleItem
                    {
                        StartDateTime=new DateTime(2019, 5, 17, 15, 0, 0),
                        EndDateTime = new DateTime(2019, 5, 17, 16, 0, 0)
                    }
                }
            };

            // Act
            var sut = new ScheduleService(this.mockLogger.Object);
            var results = sut.ExpandSchedule(mySchedule, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31));

            // assert
            Assert.Single(results);
            Assert.Equal(15, results.First().StartDateTime.Hour);
        }

        [Fact]
        public void ShouldCreateMultipleScheduleItemsSpecified()
        {
            // Act
            var sut = new ScheduleService(this.mockLogger.Object);
            var results = sut.ExpandSchedule(MultipleItemSchedule, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31));

            // Assert
            Assert.NotEmpty(results);
            Assert.Equal(2, results.Count());
            Assert.Equal(15, results.First().StartDateTime.Hour);
            Assert.Equal(17, results.Skip(1).First().StartDateTime.Hour);
        }

        [Fact]
        public void ShouldExpandWithinDatesSpecified()
        {
            // Act
            var sut = new ScheduleService(this.mockLogger.Object);
            var results = sut.ExpandSchedule(MultipleItemSchedule, new DateTime(2019, 1, 1), new DateTime(2019, 5, 18));

            // Assert
            Assert.Single(results);
        }

        [Fact]
        public void ShouldExpandWithinDelimitedDatesOnly()
        {
            var schedule = new Schedule
            {
                RecurringSchedules = new List<RecurringSchedule>
                {
                    new RecurringSchedule
                    {
                        MinStartDateTime = new DateTime(2019, 5, 1),
                        MaxEndDateTime = new DateTime(2019, 5, 31),
                        CronPattern = "0 15 * * 1",
                        Duration = TimeSpan.FromHours(1)
                    }
                }
            };

            // Act
            var sut = new ScheduleService(this.mockLogger.Object);
            var results = sut.ExpandSchedule(schedule, new DateTime(2019, 1, 1), new DateTime(2019, 12, 31));

            // assert
            Assert.Equal(4, results.Count());
            Assert.Equal(15, results.First().StartDateTime.Hour);
        }

        [Fact]
        public void ShouldExpandWithinRequestedDatesOnly()
        {
            // Arrange
            var schedule = new Schedule
            {
                RecurringSchedules = new List<RecurringSchedule>
                {
                    new RecurringSchedule
                    {
                        MinStartDateTime = new DateTime(2019, 5, 1),
                        MaxEndDateTime = new DateTime(2019, 5, 31),
                        CronPattern = "0 15 * * 1",
                        Duration = TimeSpan.FromHours(1)
                    }
                }
            };

            // Act
            var sut = new ScheduleService(this.mockLogger.Object);
            var results = sut.ExpandSchedule(schedule, new DateTime(2019, 5, 1), new DateTime(2019, 5, 15));

            // assert
            Assert.Equal(2, results.Count());
            Assert.Equal(15, results.First().StartDateTime.Hour);
        }

        [Fact]
        public void MultipleRecurringSchedules_ShouldExpandWithinRequestedDatesOnly_NoIntersection()
        {
            // Act
            var sut = new ScheduleService(this.mockLogger.Object);
            var results = sut.ExpandSchedule(MyMultipleRecurringSchedule, new DateTime(2019, 5, 1), new DateTime(2019, 5, 15));

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void ShouldExpandWithinRequestedDatesOnly_Scenario4()
        {
            // Arrange
            var schedule = new Schedule
            {
                RecurringSchedules = new List<RecurringSchedule>
                {
                    new RecurringSchedule
                    {
                        MinStartDateTime = new DateTime(2019, 5, 1),
                        MaxEndDateTime = new DateTime(2019, 5, 31),
                        CronPattern = "0 15 * * 1",
                        Duration = TimeSpan.FromHours(1)
                    }
                }
            };

            // Act
            var sut = new ScheduleService(this.mockLogger.Object);
            var results = sut.ExpandSchedule(schedule, new DateTime(2019, 4, 1), new DateTime(2019, 6, 1));

            // Assert
            Assert.Equal(4, results.Count());
            Assert.Equal(15, results.First().StartDateTime.Hour);
        }

        [Fact]
        public void ShouldExpandWithinRequestedDatesOnly_Scenario6()
        {
            // arrange
            var schedule = new Schedule
            {
                RecurringSchedules = new List<RecurringSchedule>
                {
                    new RecurringSchedule
                    {
                        MinStartDateTime = new DateTime(2019, 5, 1),
                        MaxEndDateTime = new DateTime(2019, 5, 31),
                        CronPattern = "0 15 * * 1",
                        Duration = TimeSpan.FromHours(1)
                    }
                }
            };
            //act
            var sut = new ScheduleService(this.mockLogger.Object);
            var results = sut.ExpandSchedule(schedule, new DateTime(2019, 4, 27), new DateTime(2019, 5, 24));
            //assert
            Assert.Equal(3, results.Count());
            Assert.Equal(15, results.First().StartDateTime.Hour);

            results = sut.ExpandSchedule(schedule, new DateTime(2019, 4, 27), new DateTime(2019, 5, 31));
            Assert.Equal(4, results.Count());
            Assert.Equal(15, results.First().StartDateTime.Hour);
        }
    }
}