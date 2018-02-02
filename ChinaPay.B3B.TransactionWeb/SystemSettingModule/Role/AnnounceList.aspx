<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AnnounceList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Role.AnnounceList" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link href="../../Styles/public.css?20121118" rel="stylesheet" type="text/css" />

<body>
    <form id="form1" runat="server">
     <h3 class="titleBg">公告查看</h3>
      <div class="table" id='data-list'>
         <asp:Repeater runat="server" ID="dataSource" EnableViewState="false">
            <HeaderTemplate>
              <table>
                <thead>
                  <tr>
                    <th>公告级别</th>
                    <th>标题</th>
                    <th>发布时间</th>
                    <th>操作</th>
                  </tr>
                </thead>
                <tbody>
            </HeaderTemplate>
            <ItemTemplate>
               <tr>
                 <td><%#Eval("AnnounceType")%></td>
                 <td><%#Eval("Title")%></td>
                 <td><%#Eval("PublishTime")%></td>
                 <td><a href='AnnounceInfo.aspx?Id=<%#Eval("Id") %>'>查看</a></td>
               </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody>
              </table>
            </FooterTemplate>
         </asp:Repeater>
           <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    </div>
      <div class="btns"><uc:Pager runat="server" id="pager" Visible="false"></uc:Pager></div>
    </form>
</body>
</html>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
