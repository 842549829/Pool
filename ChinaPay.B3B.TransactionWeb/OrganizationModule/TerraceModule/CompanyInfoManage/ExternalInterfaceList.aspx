<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalInterfaceList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.ExternalInterfaceList" %>
<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>接口查询</title>
</head>
    <link href="/Styles/public.css" rel="stylesheet" type="text/css" />
<body>
    <form id="form1" runat="server">
     <div class="hd">
       <h3 class="titleBg">接口查询</h3>
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
                            <span class="name">用户名：</span>
                            <asp:TextBox id="txtAccount" class="text" type="text" runat="server" />
                        </div>
                    </td>
                      <td>
                        <div class="input up-index" style="z-index: 52">
                            <span class="name">启用状态：</span>
                            <asp:DropDownList runat="server" ID="ddlStatus">
                                <asp:ListItem Value="">全部</asp:ListItem>
                                <asp:ListItem Value="1">已启用数据接口</asp:ListItem>
                                <asp:ListItem Value="0">未启用数据接口</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                    <td>
                       <div class="input">
                         <span class="name">启用时间：</span>
                           <asp:TextBox ID="txtOpenTimeStart" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtOpenTimeEnd\')}'})"></asp:TextBox>-
                           <asp:TextBox ID="txtOpenTimeEnd" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtOpenTimeStart\')}'})"></asp:TextBox>
                           <asp:HiddenField ID="hdDefaultDate" runat="server" />
                        </div>
                    </td>
                </tr>
                <tr>
                     <td>
                        <div class="input">
                            <span class="name">公司简称：</span>
                            <asp:TextBox id="txtAbbreviateName" class="text" runat="server" />
                        </div>
                    </td>
                    <td colspan="2">
                    <div class="input">
                    <asp:Button ID="btnQuery" class="btn class1" Text="查询" runat="server"  OnClientClick="SaveSearchCondition('ExternalInterfaceList')" 
                            onclick="btnQuery_Click" />
                        <input class="btn class2" type="button" value="清空条件" onclick="ResetSearchOption()" />
                        </div>
                 </td>
                </tr>
               
            </table>
        </div>
    </div>
    <div class="column table" id='data-list'>
        <asp:Repeater runat="server" ID="datalist" EnableViewState="False">
            <headertemplate>
                <table id="dataListTable">
                <thead>
                <tr>
                    <th>用户名</th>
                    <th>公司类别</th>
                    <th>公司简称</th>
                    <th>启用状态</th>
                    <th>启用时间</th>
                    <th>操作</th>
                </tr>
                </thead>
                <tbody>
            </headertemplate>
            <itemtemplate>
                <tr>
		        <td><%#Eval("UserName")%></td>
		        <td><%#Eval("CompanyTypeText")%> (<%#Eval("AccountType")%>)</td>
		        <td><%#Eval("AbbreviateName")%></td>
		        <td><%#Eval("IsOpenExternalInterface")%></td>
		        <td><%#Eval("OpenTime")%></td>
	            <td class="controls">
			        <div><a href="ExternalInterfaceSetting.aspx?CompanyId=<%#Eval("CompanyId") %>" class="el-text">设置接口</a></div>
                  </span>
		        </td>
                </tr>
            </itemtemplate>
            <footertemplate>
                </tbody></table>
            </footertemplate>
        </asp:Repeater>
    </div>
      <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>

<script src="../../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../../../Scripts/Global.js?20121120" type="text/javascript"></script>
<script src="../../../Scripts/OrderModule/QueryList.js?20121120" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        SaveDefaultData();
        $("#btnQuery").click(function () {
            var userNo = $.trim($("#txtAccount").val());
            var userNoPattern = /^\d$/;
            if (userNo.length > 30) {
                alert("用户名字数不能超过30！");
                $("#txtAccount").select();
                return false;
            }
            if (userNoPattern.test(userNo)) {
                alert("用户名格式错误！");
                $("#txtAccount").select();
                return false;
            }
            var abbreviateName = $.trim($("#txtAbbreviateName").val());
            var abbreviateNamePattern = /^[\u4e00-\u9fa5\w][\u4e00-\u9fa5\w\s\.,]*$/;
            if (abbreviateName.length > 10) {
                alert("公司简称字数不能超过10！");
                $("#txtAbbreviateName").select();
                return false;
            }
            if (abbreviateNamePattern.test(abbreviateName)) {
                alert("公司简称格式错误！");
                $("#txtAbbreviateName").select();
                return false;
            }
        });
        pageName = 'ExternalInterfaceList';
    })
</script>
