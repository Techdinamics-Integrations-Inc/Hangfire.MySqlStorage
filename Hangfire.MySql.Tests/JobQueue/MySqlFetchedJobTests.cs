using System;
using Hangfire.MySql.JobQueue;
using Moq;
using Xunit;

namespace Hangfire.MySql.Tests.JobQueue
{
    public class MySqlFetchedJobTests : IClassFixture<TestDatabaseFixture>
    {
        private const int JobId = 1;
        private const string Queue = "queue";

        private readonly Mock<MySqlStorage> _storage;
        private readonly int _id = 0;
        private readonly FetchedJob _fetchedJob;
        private readonly MySqlStorageOptions _storageOptions;

        public MySqlFetchedJobTests()
        {
            _fetchedJob = new FetchedJob(){Id = _id, JobId = JobId, Queue = Queue};
            _storageOptions = new MySqlStorageOptions { PrepareSchemaIfNecessary = false };
            _storage = new Mock<MySqlStorage>(ConnectionUtils.GetConnectionString(), _storageOptions);
        }

        [Fact]
        public void Ctor_ThrowsAnException_WhenStorageIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MySqlFetchedJob(null, _fetchedJob, _storageOptions));

            Assert.Equal("storage", exception.ParamName);
        }

        [Fact]
        public void Ctor_ThrowsAnException_WhenFetchedJobIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new MySqlFetchedJob(_storage.Object, null, _storageOptions));

            Assert.Equal("fetchedJob", exception.ParamName);
        }

        [Fact]
        public void Ctor_CorrectlySets_AllInstanceProperties()
        {
            var fetchedJob = new MySqlFetchedJob(_storage.Object, _fetchedJob, _storageOptions);

            Assert.Equal(JobId.ToString(), fetchedJob.JobId);
            Assert.Equal(Queue, fetchedJob.Queue);
        }

        private MySqlFetchedJob CreateFetchedJob(int jobId, string queue)
        {
            return new MySqlFetchedJob(_storage.Object,
                new FetchedJob() {JobId = jobId, Queue = queue, Id = _id}, _storageOptions);
        }
    }
}
