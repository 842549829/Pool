<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DistributionOemAuthorizationList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.DistributionOemAuthorizationList" %>

<%@ Import Namespace="ChinaPay.B3B.Common.Enums" %>
<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
 </head>    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link href="/Styles/masklayer/masklayer.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .maxWidth {
            max-width: 80px;
            word-break: break-all;
            word-wrap: break-word;   
        }
        td.controls {
            width: 260px;
            padding-left: 60px;
        }
        td.controls div {
            width: 60px;
            float: left;   
       }
        #dataListTable td {
            vertical-align: middle;   
        }.dlLeft
        {
            float:left;
        }
        .warn
        {
            color:Red;
        }        
    </style>

<body>
   <form id="form1" defaultbutton="btnQuery" runat="server">
    <div class="hd">
        <h3 class="titleBg">
            授权管理</h3>
    </div>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
                </colgroup>
                <tr>
                <td>
                        <div class="input">
                            <span class="name">授权时间：</span>
                            <asp:TextBox ID="txtBeginTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtEndTime\')}'})"></asp:TextBox>&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;<asp:TextBox
                            ID="txtEndTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtBeginTime\')}'})"></asp:TextBox>
                            <asp:HiddenField ID="hdDefaultDate" runat="server" />
                        </div>
                    </td>
                     <td>
                        <div class="input">
                            <span class="name">用户名：</span>
                            <asp:TextBox id="txtAccount" class="text" type="text" runat="server" />
                        </div>
                    </td>
                    <td>
                      <div class="input">
                       <span class="name">授权状态：</span>
                       <asp:DropDownList ID="ddlAuthorizationStatus" runat="server">
                        <asp:ListItem Text="所有" Value=""></asp:ListItem>
                        <asp:ListItem Text="正常" Value="1"></asp:ListItem>
                        <asp:ListItem Text="失效" Value="0"></asp:ListItem>
                       </asp:DropDownList>
                      </div>
                    </td>
                   
                </tr>
                <tr>
                   
                    <td>
                        <div class="input">
                            <span class="name">授权域名：</span><asp:TextBox id="txtDomainName" class="text" runat="server" />
                        </div>
                    </td>
                     <td>
                        <div class="input">
                            <span class="name">公司简称：</span>
                            <asp:TextBox id="txtAbbreviateName" class="text" runat="server" />
                        </div>
                    </td>
                    <td>
                       <div class="input">
                          <span class="name"> OEM名称：</span>
                         <asp:TextBox ID="txtOemName" runat="server" class="text" ></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr class="btns">
                 <td colspan="3">
                    <asp:Button ID="btnQuery" class="btn class1" Text="查询" runat="server" 
                         OnClientClick="SaveSearchCondition('CompanyAuthorizationList')" onclick="btnQuery_Click" />
                        <input class="btn class2" type="button" value="新增授权" onclick="javascript:window.location.href='DistributionOemAuthorizationAddOrUpdate.aspx'" />
                        <input class="btn class2" type="button" value="清空条件" onclick="ResetSearchOption()" />
                 </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="column table" id='data-list'>
        <asp:Repeater runat="server" ID="datalist" OnPreRender="AddEmptyTemplate">
            <headertemplate>
                <table id="dataListTable">
                <colgroup>
                    <col />
                    <col span="5" class="w10"/>
                    <col />
                </colgroup>
                <thead>
                <tr>
                    <th>授权时间</th>
                    <th>用户名</th>
                    <th>公司简称</th>
                    <th>OEM名称</th>
                    <th>授权域名</th>
                    <th>授权状态</th>
                    <th>操作</th>
                </tr>
                </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
		        <td><%#Eval("RegisterTime")%></td>
		        <td><%#Eval("UserNo")%></td>
		        <td><%#Eval("AbbreviateName")%></td>
		        <td><%#Eval("SiteName")%></td>
		        <td><%#Eval("DomainName")%></td>
                <td><%#Eval("Status")%></td>
	            <td class="controls">
                   <a href='DistributionOemAuthorizationDetail.aspx?OemId=<%#Eval("Id") %>&UserNo=<%#Eval("UserNo") %>'>查看详情</a>
			       <a href='DistributionOemAuthorizationAddOrUpdate.aspx?OemId=<%#Eval("Id") %>&UserNo=<%#Eval("UserNo") %>' class="el-text">修改授权</a>
                   <a href='AirlineConfigSettings.aspx?OemId=<%#Eval("Id") %>&UserNo=<%#Eval("UserNo") %>' class="el-text">配置管理</a>
                   <br />
			       <a href='DistributionOEMSiteSet.aspx?id=<%#Eval("Company") %>' class="el-text">修改站点信息</a>
			       <a href='DistributionOEMSiteSetHeader.aspx?id=<%#Eval("Company") %>' class="el-text">修改页头页脚信息</a>
                   <a href='/SystemSettingModule/OEMContract.aspx?CompanyId=<%# Eval("Company") %>'>联系方式管理</a>
		        </td>
                </tr>
            </itemtemplate>
            <footertemplate>
                </tbody></table>
            </footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        SaveDefaultData();
        pageName = 'CompanyAuthorizationList';
    })
</script>