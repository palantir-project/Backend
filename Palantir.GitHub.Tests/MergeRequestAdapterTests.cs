namespace Palantir.GitHub.Tests
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

        private GitHubPullRequest adaptee;

        public MergeRequestAdapterTests()
        {
            // Given
            string gitHubResponse = "[{'url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls/1','id':315229289,'node_id':'MDExOlB1bGxSZXF1ZXN0MzE1MjI5Mjg5','html_url':'https://github.com/gonzalocozzi/rest-api-test/pull/1','diff_url':'https://github.com/gonzalocozzi/rest-api-test/pull/1.diff','patch_url':'https://github.com/gonzalocozzi/rest-api-test/pull/1.patch','issue_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues/1','number':1,'state':'open','locked':false,'title':'Test','user':{'login':'gonzalocozzi','id':13871650,'node_id':'MDQ6VXNlcjEzODcxNjUw','avatar_url':'https://avatars0.githubusercontent.com/u/13871650?v=4','gravatar_id':'','url':'https://api.github.com/users/gonzalocozzi','html_url':'https://github.com/gonzalocozzi','followers_url':'https://api.github.com/users/gonzalocozzi/followers','following_url':'https://api.github.com/users/gonzalocozzi/following{/other_user}','gists_url':'https://api.github.com/users/gonzalocozzi/gists{/gist_id}','starred_url':'https://api.github.com/users/gonzalocozzi/starred{/owner}{/repo}','subscriptions_url':'https://api.github.com/users/gonzalocozzi/subscriptions','organizations_url':'https://api.github.com/users/gonzalocozzi/orgs','repos_url':'https://api.github.com/users/gonzalocozzi/repos','events_url':'https://api.github.com/users/gonzalocozzi/events{/privacy}','received_events_url':'https://api.github.com/users/gonzalocozzi/received_events','type':'User','site_admin':false},'body':'','created_at':'2019-09-07T22:26:59Z','updated_at':'2019-09-23T23:46:10Z','closed_at':null,'merged_at':null,'merge_commit_sha':'63ab265e0edecce1ad0b32a136ce363bfc45d856','assignee':null,'assignees':[],'requested_reviewers':[],'requested_teams':[],'labels':[],'milestone':null,'commits_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls/1/commits','review_comments_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls/1/comments','review_comment_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls/comments{/number}','comments_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues/1/comments','statuses_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/statuses/abb48e2541a6f1da6c50783dbe2eb7a80e6e4256','head':{'label':'gonzalocozzi:test','ref':'test','sha':'abb48e2541a6f1da6c50783dbe2eb7a80e6e4256','user':{'login':'gonzalocozzi','id':13871650,'node_id':'MDQ6VXNlcjEzODcxNjUw','avatar_url':'https://avatars0.githubusercontent.com/u/13871650?v=4','gravatar_id':'','url':'https://api.github.com/users/gonzalocozzi','html_url':'https://github.com/gonzalocozzi','followers_url':'https://api.github.com/users/gonzalocozzi/followers','following_url':'https://api.github.com/users/gonzalocozzi/following{/other_user}','gists_url':'https://api.github.com/users/gonzalocozzi/gists{/gist_id}','starred_url':'https://api.github.com/users/gonzalocozzi/starred{/owner}{/repo}','subscriptions_url':'https://api.github.com/users/gonzalocozzi/subscriptions','organizations_url':'https://api.github.com/users/gonzalocozzi/orgs','repos_url':'https://api.github.com/users/gonzalocozzi/repos','events_url':'https://api.github.com/users/gonzalocozzi/events{/privacy}','received_events_url':'https://api.github.com/users/gonzalocozzi/received_events','type':'User','site_admin':false},'repo':{'id':207030759,'node_id':'MDEwOlJlcG9zaXRvcnkyMDcwMzA3NTk=','name':'rest-api-test','full_name':'gonzalocozzi/rest-api-test','private':true,'owner':{'login':'gonzalocozzi','id':13871650,'node_id':'MDQ6VXNlcjEzODcxNjUw','avatar_url':'https://avatars0.githubusercontent.com/u/13871650?v=4','gravatar_id':'','url':'https://api.github.com/users/gonzalocozzi','html_url':'https://github.com/gonzalocozzi','followers_url':'https://api.github.com/users/gonzalocozzi/followers','following_url':'https://api.github.com/users/gonzalocozzi/following{/other_user}','gists_url':'https://api.github.com/users/gonzalocozzi/gists{/gist_id}','starred_url':'https://api.github.com/users/gonzalocozzi/starred{/owner}{/repo}','subscriptions_url':'https://api.github.com/users/gonzalocozzi/subscriptions','organizations_url':'https://api.github.com/users/gonzalocozzi/orgs','repos_url':'https://api.github.com/users/gonzalocozzi/repos','events_url':'https://api.github.com/users/gonzalocozzi/events{/privacy}','received_events_url':'https://api.github.com/users/gonzalocozzi/received_events','type':'User','site_admin':false},'html_url':'https://github.com/gonzalocozzi/rest-api-test','description':null,'fork':false,'url':'https://api.github.com/repos/gonzalocozzi/rest-api-test','forks_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/forks','keys_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/keys{/key_id}','collaborators_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/collaborators{/collaborator}','teams_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/teams','hooks_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/hooks','issue_events_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues/events{/number}','events_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/events','assignees_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/assignees{/user}','branches_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/branches{/branch}','tags_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/tags','blobs_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/blobs{/sha}','git_tags_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/tags{/sha}','git_refs_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/refs{/sha}','trees_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/trees{/sha}','statuses_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/statuses/{sha}','languages_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/languages','stargazers_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/stargazers','contributors_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/contributors','subscribers_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/subscribers','subscription_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/subscription','commits_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/commits{/sha}','git_commits_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/commits{/sha}','comments_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/comments{/number}','issue_comment_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues/comments{/number}','contents_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/contents/{+path}','compare_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/compare/{base}...{head}','merges_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/merges','archive_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/{archive_format}{/ref}','downloads_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/downloads','issues_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues{/number}','pulls_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls{/number}','milestones_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/milestones{/number}','notifications_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/notifications{?since,all,participating}','labels_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/labels{/name}','releases_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/releases{/id}','deployments_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/deployments','created_at':'2019-09-07T22:04:22Z','updated_at':'2019-12-19T01:46:19Z','pushed_at':'2019-09-23T23:46:11Z','git_url':'git://github.com/gonzalocozzi/rest-api-test.git','ssh_url':'git@github.com:gonzalocozzi/rest-api-test.git','clone_url':'https://github.com/gonzalocozzi/rest-api-test.git','svn_url':'https://github.com/gonzalocozzi/rest-api-test','homepage':null,'size':3258,'stargazers_count':0,'watchers_count':0,'language':'JavaScript','has_issues':true,'has_projects':true,'has_downloads':true,'has_wiki':true,'has_pages':false,'forks_count':0,'mirror_url':null,'archived':false,'disabled':false,'open_issues_count':1,'license':null,'forks':0,'open_issues':1,'watchers':0,'default_branch':'master'}},'base':{'label':'gonzalocozzi:master','ref':'master','sha':'f63a5480b0591692da53f24044acd941c89be724','user':{'login':'gonzalocozzi','id':13871650,'node_id':'MDQ6VXNlcjEzODcxNjUw','avatar_url':'https://avatars0.githubusercontent.com/u/13871650?v=4','gravatar_id':'','url':'https://api.github.com/users/gonzalocozzi','html_url':'https://github.com/gonzalocozzi','followers_url':'https://api.github.com/users/gonzalocozzi/followers','following_url':'https://api.github.com/users/gonzalocozzi/following{/other_user}','gists_url':'https://api.github.com/users/gonzalocozzi/gists{/gist_id}','starred_url':'https://api.github.com/users/gonzalocozzi/starred{/owner}{/repo}','subscriptions_url':'https://api.github.com/users/gonzalocozzi/subscriptions','organizations_url':'https://api.github.com/users/gonzalocozzi/orgs','repos_url':'https://api.github.com/users/gonzalocozzi/repos','events_url':'https://api.github.com/users/gonzalocozzi/events{/privacy}','received_events_url':'https://api.github.com/users/gonzalocozzi/received_events','type':'User','site_admin':false},'repo':{'id':207030759,'node_id':'MDEwOlJlcG9zaXRvcnkyMDcwMzA3NTk=','name':'rest-api-test','full_name':'gonzalocozzi/rest-api-test','private':true,'owner':{'login':'gonzalocozzi','id':13871650,'node_id':'MDQ6VXNlcjEzODcxNjUw','avatar_url':'https://avatars0.githubusercontent.com/u/13871650?v=4','gravatar_id':'','url':'https://api.github.com/users/gonzalocozzi','html_url':'https://github.com/gonzalocozzi','followers_url':'https://api.github.com/users/gonzalocozzi/followers','following_url':'https://api.github.com/users/gonzalocozzi/following{/other_user}','gists_url':'https://api.github.com/users/gonzalocozzi/gists{/gist_id}','starred_url':'https://api.github.com/users/gonzalocozzi/starred{/owner}{/repo}','subscriptions_url':'https://api.github.com/users/gonzalocozzi/subscriptions','organizations_url':'https://api.github.com/users/gonzalocozzi/orgs','repos_url':'https://api.github.com/users/gonzalocozzi/repos','events_url':'https://api.github.com/users/gonzalocozzi/events{/privacy}','received_events_url':'https://api.github.com/users/gonzalocozzi/received_events','type':'User','site_admin':false},'html_url':'https://github.com/gonzalocozzi/rest-api-test','description':null,'fork':false,'url':'https://api.github.com/repos/gonzalocozzi/rest-api-test','forks_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/forks','keys_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/keys{/key_id}','collaborators_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/collaborators{/collaborator}','teams_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/teams','hooks_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/hooks','issue_events_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues/events{/number}','events_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/events','assignees_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/assignees{/user}','branches_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/branches{/branch}','tags_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/tags','blobs_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/blobs{/sha}','git_tags_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/tags{/sha}','git_refs_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/refs{/sha}','trees_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/trees{/sha}','statuses_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/statuses/{sha}','languages_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/languages','stargazers_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/stargazers','contributors_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/contributors','subscribers_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/subscribers','subscription_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/subscription','commits_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/commits{/sha}','git_commits_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/git/commits{/sha}','comments_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/comments{/number}','issue_comment_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues/comments{/number}','contents_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/contents/{+path}','compare_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/compare/{base}...{head}','merges_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/merges','archive_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/{archive_format}{/ref}','downloads_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/downloads','issues_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues{/number}','pulls_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls{/number}','milestones_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/milestones{/number}','notifications_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/notifications{?since,all,participating}','labels_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/labels{/name}','releases_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/releases{/id}','deployments_url':'https://api.github.com/repos/gonzalocozzi/rest-api-test/deployments','created_at':'2019-09-07T22:04:22Z','updated_at':'2019-12-19T01:46:19Z','pushed_at':'2019-09-23T23:46:11Z','git_url':'git://github.com/gonzalocozzi/rest-api-test.git','ssh_url':'git@github.com:gonzalocozzi/rest-api-test.git','clone_url':'https://github.com/gonzalocozzi/rest-api-test.git','svn_url':'https://github.com/gonzalocozzi/rest-api-test','homepage':null,'size':3258,'stargazers_count':0,'watchers_count':0,'language':'JavaScript','has_issues':true,'has_projects':true,'has_downloads':true,'has_wiki':true,'has_pages':false,'forks_count':0,'mirror_url':null,'archived':false,'disabled':false,'open_issues_count':1,'license':null,'forks':0,'open_issues':1,'watchers':0,'default_branch':'master'}},'_links':{'self':{'href':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls/1'},'html':{'href':'https://github.com/gonzalocozzi/rest-api-test/pull/1'},'issue':{'href':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues/1'},'comments':{'href':'https://api.github.com/repos/gonzalocozzi/rest-api-test/issues/1/comments'},'review_comments':{'href':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls/1/comments'},'review_comment':{'href':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls/comments{/number}'},'commits':{'href':'https://api.github.com/repos/gonzalocozzi/rest-api-test/pulls/1/commits'},'statuses':{'href':'https://api.github.com/repos/gonzalocozzi/rest-api-test/statuses/abb48e2541a6f1da6c50783dbe2eb7a80e6e4256'}},'author_association':'OWNER'}]";

            IEnumerable<GitHubPullRequest> gitHubPullRequest = JsonConvert.DeserializeObject<IEnumerable<GitHubPullRequest>>(gitHubResponse);
            this.adaptee = gitHubPullRequest.FirstOrDefault();

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

            Assert.Equal(315229289, adaptedId);
        }

        [Fact]
        public void ReturnsAdaptedTitleField()
        {
            // Then
            string adaptedTitle = this.expectedMergeRequest.Title;

            Assert.Equal("Test", adaptedTitle);
        }

        [Fact]
        public void ReturnsAdaptedAuthorNameField()
        {
            // Then
            string adaptedAuthorName = this.expectedMergeRequest.AuthorName;

            Assert.Equal("gonzalocozzi", adaptedAuthorName);
        }

        [Fact]
        public void ReturnsAdaptedUrlField()
        {
            // Then
            string adaptedUrl = this.expectedMergeRequest.Url.ToString();

            Assert.Equal("https://github.com/gonzalocozzi/rest-api-test/pull/1", adaptedUrl);
        }

        [Fact]
        public void ReturnsAdaptedCreationDateField()
        {
            // Then
            DateTimeOffset adaptedCreationDate = this.expectedMergeRequest.CreationDate;

            Assert.Equal("2019-09-07 22:26:59Z", adaptedCreationDate.ToString("u"));
        }

        [Fact]
        public void ReturnsAdaptedLastUpdateDateField()
        {
            // Then
            DateTimeOffset adaptedLastUpdateDate = this.expectedMergeRequest.LastUpdate;

            Assert.Equal("2019-09-23 23:46:10Z", adaptedLastUpdateDate.ToString("u"));
        }

        [Fact]
        public void ReturnsAdaptedStateField()
        {
            // Then
            string adaptedState = this.expectedMergeRequest.State;

            Assert.Equal("open", adaptedState);
        }
    }
}