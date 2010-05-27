<%@ Import Namespace="System.ServiceModel.Syndication" %>
<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<SyndicationFeed>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="Content" runat="server">
<% foreach (var item in Model.Items)
   { %>
<div class="article">
<h1><a href="#"><%= item.Title.Text %></a></h1>
<p class="timestamp"><%= item.LastUpdatedTime.LocalDateTime.ToString()%></p>
<%= item.Content.GetText() %>
</div>
<% } %>
</asp:Content>
