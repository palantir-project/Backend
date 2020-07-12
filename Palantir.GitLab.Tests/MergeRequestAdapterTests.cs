namespace Palantir.GitLab.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Palantir.Domain.Models;
    using Xunit;

    public class MergeRequestAdapterTests : IDisposable
    {
        private readonly MergeRequest expectedMergeRequest;

        private GitLabMergeRequest adaptee;

        public MergeRequestAdapterTests()
        {
            // Given
            string gitLabResponse = "[{'id':44126460,'iid':1,'project_id':15722380,'title':'Readme updated','description':'','state':'opened','created_at':'2019-12-06T12:55:01.316Z','updated_at':'2019-12-06T12:55:01.316Z','merged_by':null,'merged_at':null,'closed_by':null,'closed_at':null,'target_branch':'master','source_branch':'develop','user_notes_count':0,'upvotes':0,'downvotes':0,'assignee':null,'author':{'id':108019,'name':'Nicolas','username':'nicopaez','state':'active','avatar_url':'https://secure.gravatar.com/avatar/f4c52cfd67c861fd5c87c66704b88b3c?s=80&d=identicon','web_url':'https://gitlab.com/nicopaez'},'assignees':[],'source_project_id':15722380,'target_project_id':15722380,'labels':[],'work_in_progress':false,'milestone':null,'merge_when_pipeline_succeeds':false,'merge_status':'can_be_merged','sha':'0520da5b424a18e7c3af2318fa7141ee6e927b8f','merge_commit_sha':null,'squash_commit_sha':null,'discussion_locked':null,'should_remove_source_branch':null,'force_remove_source_branch':true,'reference':'!1','web_url':'https://gitlab.com/untref-ingsoft/tfi-cozzi/sample-repo1/merge_requests/1','time_stats':{'time_estimate':0,'total_time_spent':0,'human_time_estimate':null,'human_total_time_spent':null},'squash':false,'task_completion_status':{'count':0,'completed_count':0},'has_conflicts':false,'blocking_discussions_resolved':true,'approvals_before_merge':0}]";

            IEnumerable<GitLabMergeRequest> gitLabMergeRequest = JsonConvert.DeserializeObject<IEnumerable<GitLabMergeRequest>>(gitLabResponse);
            this.adaptee = gitLabMergeRequest.FirstOrDefault();

            // When
            MergeRequestAdapter adapter = new MergeRequestAdapter();
            this.expectedMergeRequest = adapter.GetMergeRequest(this.adaptee);
        }

        public void Dispose()
        {
            this.adaptee = null;
        }

        [Fact]
        public void ReturnsAdaptedIdField()
        {
            // Then
            long adaptedId = this.expectedMergeRequest.Id;

            Assert.Equal(44126460, adaptedId);
        }

        [Fact]
        public void ReturnsAdaptedTitleField()
        {
            // Then
            string adaptedTitle = this.expectedMergeRequest.Title;

            Assert.Equal("Readme updated", adaptedTitle);
        }

        [Fact]
        public void ReturnsAdaptedAuthorNameField()
        {
            // Then
            string adaptedAuthorName = this.expectedMergeRequest.AuthorName;

            Assert.Equal("nicopaez", adaptedAuthorName);
        }

        [Fact]
        public void ReturnsAdaptedUrlField()
        {
            // Then
            string adaptedUrl = this.expectedMergeRequest.Url.ToString();

            Assert.Equal("https://gitlab.com/untref-ingsoft/tfi-cozzi/sample-repo1/merge_requests/1", adaptedUrl);
        }

        [Fact]
        public void ReturnsAdaptedCreationDateField()
        {
            // Then
            DateTimeOffset adaptedCreationDate = this.expectedMergeRequest.CreationDate;

            Assert.Equal("2019-12-06 12:55:01Z", adaptedCreationDate.ToString("u"));
        }

        [Fact]
        public void ReturnsAdaptedLastUpdateDateField()
        {
            // Then
            DateTimeOffset adaptedLastUpdateDate = this.expectedMergeRequest.LastUpdate;

            Assert.Equal("2019-12-06 12:55:01Z", adaptedLastUpdateDate.ToString("u"));
        }

        [Fact]
        public void ReturnsAdaptedStateField()
        {
            // Then
            string adaptedState = this.expectedMergeRequest.State;

            Assert.Equal("opened", adaptedState);
        }
    }
}