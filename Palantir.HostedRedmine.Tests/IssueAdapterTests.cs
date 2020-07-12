namespace Palantir.HostedRedmine.Tests
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Palantir.Domain.Models;
    using Xunit;

    public class IssueAdapterTests : IDisposable
    {
        private readonly Issue expectedIssue;

        private HostedRedmineIssue adaptee;

        public IssueAdapterTests()
        {
            // Given
            string hostedRedmineResponse = "{\"issues\":[{\"id\":853616,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118916,\"name\":\"raj dhruw\"},\"subject\":\"media\",\"description\":\"i can't spend my site plz. \r\ncheck my site. \u003ca href=https://www.socialmediagossips.com/allison-parker/\u003e allison-parker \u003c/a\u003e\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-12-23T07:15:20Z\",\"updated_on\":\"2019-12-23T07:15:20Z\",\"closed_on\":null},{\"id\":852351,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118699,\"name\":\"cordy daymen\"},\"subject\":\"Quickbooks tech support\",\"description\":\"Quickbooks provides best accounting features for creating and managing accounts details and records for the business and firms, Quickbooks is mostly used by businessmen and professionals for there office work and in case you need any help assistance contact Quickbooks tech support and ask for Quickbooks assistance\r\nvisit us at [[https://qbsolutions.co/]]\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-12-13T10:27:45Z\",\"updated_on\":\"2019-12-13T10:27:45Z\",\"closed_on\":null},{\"id\":852350,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118699,\"name\":\"cordy daymen\"},\"subject\":\"Quickbooks tech support\",\"description\":\"Quickbooks provides best accounting features for creating and managing accounts details and records for the business and firms, Quickbooks is mostly used by businessmen and professionals for there office work and in case you need any help assistance contact Quickbooks tech support and ask for [[https://qbsolutions.co/]]\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-12-13T10:27:23Z\",\"updated_on\":\"2019-12-13T10:27:23Z\",\"closed_on\":null},{\"id\":852349,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118699,\"name\":\"cordy daymen\"},\"subject\":\"Quickbooks tech support\",\"description\":\"Quickbooks provides best accounting features for creating and managing accounts details and records for the business and firms, Quickbooks is mostly used by businessmen and professionals for there office work and in case you need any help assistance contact Quickbooks tech support and ask for [[Quickbooks assistance]https://qbsolutions.co/]\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-12-13T10:27:00Z\",\"updated_on\":\"2019-12-13T10:27:00Z\",\"closed_on\":null},{\"id\":852347,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118699,\"name\":\"cordy daymen\"},\"subject\":\"Quickbooks tech support\",\"description\":\"Quickbooks provides best accounting features for creating and managing accounts details and records for the business and firms, Quickbooks is mostly used by businessmen and professionals for there office work and in case you need any help assistance contact Quickbooks tech support and ask for [[Quickbooks assistance]]https://qbsolutions.co/\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-12-13T10:26:20Z\",\"updated_on\":\"2019-12-13T10:26:20Z\",\"closed_on\":null},{\"id\":851196,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":8,\"name\":\"In Progress\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":74416,\"name\":\"Eric Chau\"},\"assigned_to\":{\"id\":103459,\"name\":\"surendra k\"},\"subject\":\"[Front][Back] Export Data table\",\"description\":\"Provide ability to download in CSV format the tables mentioned in attached file\r\n\r\n1. When a row is expandable in the web interface, the corresponding data is stored as a json in the CSV file.\r\nFor example Merchant Transaction Log : \r\n- each row represent a Merchant Balance Order\r\n- each row will have a column *payment*, this column will be an object with Merchant Balance Payment info\r\n\r\n2. When a table has a column *status*, for example Merchant Transaction Log\r\n- use the computed status (PAID | UNPAID | REQUESTED | CANCELLED)\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-12-05T17:59:24Z\",\"updated_on\":\"2019-12-11T20:09:08Z\",\"closed_on\":null},{\"id\":849782,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":55625,\"name\":\"Александр Алексеев\"},\"assigned_to\":{\"id\":55621,\"name\":\"Kate Maksimova\"},\"subject\":\"ТЗ-1 \",\"description\":\"https://docs.google.com/document/d/1iJ9RfyCJJIKWVPZnDHYzg1zYhEsM15sY9XqMiodQI9k/edit\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":4.0,\"created_on\":\"2019-11-27T11:05:15Z\",\"updated_on\":\"2019-12-01T15:17:04Z\",\"closed_on\":null},{\"id\":849666,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"aaaa  :wNew API: create/update/delete file\",\"description\":\"Create new API for manipulations with repository\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-26T15:47:36Z\",\"updated_on\":\"2019-11-26T15:47:36Z\",\"closed_on\":null},{\"id\":849572,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"aaaa  :wNew API: create/update/delete file\",\"description\":\"Create new API for manipulations with repository\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-26T10:32:41Z\",\"updated_on\":\"2019-11-26T10:32:41Z\",\"closed_on\":null},{\"id\":849547,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"New API: create/update/delete file\",\"description\":\"Create new API for manipulations with repository\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-26T09:47:44Z\",\"updated_on\":\"2019-12-25T15:21:04Z\",\"closed_on\":null},{\"id\":849297,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-25T14:06:36Z\",\"updated_on\":\"2019-12-25T16:27:24Z\",\"closed_on\":null},{\"id\":849152,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T16:38:45Z\",\"updated_on\":\"2019-11-24T16:38:45Z\",\"closed_on\":null},{\"id\":849150,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T16:32:45Z\",\"updated_on\":\"2019-12-25T01:48:40Z\",\"closed_on\":null},{\"id\":849149,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T16:32:24Z\",\"updated_on\":\"2019-12-25T15:52:05Z\",\"closed_on\":null},{\"id\":849145,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T15:51:04Z\",\"updated_on\":\"2019-11-24T15:51:05Z\",\"closed_on\":null},{\"id\":849143,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T15:32:12Z\",\"updated_on\":\"2019-11-25T14:06:38Z\",\"closed_on\":null},{\"id\":849141,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T15:27:38Z\",\"updated_on\":\"2019-11-24T15:27:38Z\",\"closed_on\":null},{\"id\":849140,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T15:26:24Z\",\"updated_on\":\"2019-11-24T15:26:24Z\",\"closed_on\":null},{\"id\":849130,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T14:33:39Z\",\"updated_on\":\"2019-11-24T15:30:12Z\",\"closed_on\":null},{\"id\":849129,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T14:32:26Z\",\"updated_on\":\"2019-11-24T14:32:26Z\",\"closed_on\":null},{\"id\":849128,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"this is a test title\",\"description\":\"this is my description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T14:32:02Z\",\"updated_on\":\"2019-11-24T14:32:02Z\",\"closed_on\":null},{\"id\":849127,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"first subject\",\"description\":\"description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T14:29:30Z\",\"updated_on\":\"2019-11-24T14:29:30Z\",\"closed_on\":null},{\"id\":849114,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":4,\"name\":\"Normal\"},\"author\":{\"id\":118217,\"name\":\"River Lin\"},\"subject\":\"first subject\",\"description\":\"description\",\"start_date\":null,\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-24T13:33:48Z\",\"updated_on\":\"2019-11-24T13:33:48Z\",\"closed_on\":null},{\"id\":846973,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":5,\"name\":\"High\"},\"author\":{\"id\":117830,\"name\":\"John Depp\"},\"subject\":\"Spirit Airlines Reservations Number\",\"description\":\"Hello friends my name is John Depp. I am working in Spirit Airlines Reservations Numbers. Our company provide you best reservation service.  For any flight booking related query call at spirit airlines reservations Number. Our executives provide you best services. Our services are available 24/7 via toll free Number.http://spirit.airlinesreservationsnumbers.com\",\"start_date\":\"1990-03-20\",\"due_date\":null,\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-13T06:29:49Z\",\"updated_on\":\"2019-11-13T06:29:49Z\",\"closed_on\":null},{\"id\":846262,\"project\":{\"id\":1869,\"name\":\"TestCreditSystem\"},\"tracker\":{\"id\":4,\"name\":\"Task\"},\"status\":{\"id\":1,\"name\":\"New\"},\"priority\":{\"id\":5,\"name\":\"High\"},\"author\":{\"id\":74279,\"name\":\"Alexander Rudenko\"},\"assigned_to\":{\"id\":74279,\"name\":\"Alexander Rudenko\"},\"subject\":\"Buy and hang TV bracket on the wall\",\"description\":\"1. Find a TV bracket having appropriate price and quality.\r\n2. Buy it.\r\n3. Measure the TV set dimensions.\r\n4. Hang it on the wall.\",\"start_date\":\"2019-11-08\",\"due_date\":\"2019-11-30\",\"done_ratio\":0,\"is_private\":false,\"estimated_hours\":null,\"created_on\":\"2019-11-07T21:25:37Z\",\"updated_on\":\"2019-11-07T21:44:18Z\",\"closed_on\":null}],\"total_count\":199,\"offset\":0,\"limit\":25}";

            HostedRedmineIssue hostedRedmineIssue = JsonConvert.DeserializeObject<HostedRedmineIssue>(hostedRedmineResponse);
            this.adaptee = hostedRedmineIssue;

            // When
            IssueAdapter adapter = new IssueAdapter();
            this.expectedIssue = adapter.GetIssue(this.adaptee.Issues.First());
        }

        public void Dispose()
        {
            this.adaptee = null;
        }

        [Fact]
        public void ReturnsAdaptedIdField()
        {
            // Then
            long adaptedId = this.expectedIssue.Id;

            Assert.Equal(853616, adaptedId);
        }

        [Fact]
        public void ReturnsAdaptedUrlField()
        {
            // Then
            string adaptedUrl = this.expectedIssue.Url.ToString();

            Assert.Equal("http://www.hostedredmine.com/issues/853616", adaptedUrl);
        }

        [Fact]
        public void ReturnsAdaptedProjectField()
        {
            // Then
            string adaptedProject = this.expectedIssue.Project;

            Assert.Equal("TestCreditSystem", adaptedProject);
        }

        [Fact]
        public void ReturnsAdaptedTitleField()
        {
            // Then
            string adaptedTitle = this.expectedIssue.Title;

            Assert.Equal("media", adaptedTitle);
        }

        [Fact]
        public void ReturnsAdaptedStateField()
        {
            // Then
            string adaptedState = this.expectedIssue.State;

            Assert.Equal("New", adaptedState);
        }

        [Fact]
        public void ReturnsAdaptedAuthorNameField()
        {
            // Then
            string adaptedAuthorName = this.expectedIssue.AuthorName;

            Assert.Equal("raj dhruw", adaptedAuthorName);
        }

        [Fact]
        public void ReturnsAdaptedCreationDateField()
        {
            // Then
            DateTimeOffset adaptedCreationDate = this.expectedIssue.CreatedOn;

            Assert.Equal("2019-12-23 07:15:20Z", adaptedCreationDate.ToString("u"));
        }

        [Fact]
        public void ReturnsAdaptedUpdateDateField()
        {
            // Then
            DateTimeOffset adaptedCreationDate = this.expectedIssue.UpdatedOn;

            Assert.Equal("2019-12-23 07:15:20Z", adaptedCreationDate.ToString("u"));
        }
    }
}