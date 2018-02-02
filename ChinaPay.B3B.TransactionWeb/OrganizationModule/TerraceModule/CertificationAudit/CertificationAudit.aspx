<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CertificationAudit.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CertificationAudit.CertificationAudit" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>认证中心</title>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnSearch">
    <h3 class="titleBg">
        认证中心</h3>
    <div class="box-a">
        <table class="condition">
            <colgroup>
                <col class="w30" />
                    <col class="w35" />
                    <col class="w35" />
            </colgroup>
            <tr>
                <td>
                    <div class="input">
                        <span class="name">用户名：</span>
                        <asp:TextBox ID="txtAccount" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="input up-index" style="z-index: 53">
                        <span class="name fl">公司类别：</span>
                        <div class="dlLeft fl">
                            <asp:DropDownList ID="ddlRoleType" runat="server" Width="78px">
                            </asp:DropDownList>
                        </div>
                        <div class="dlLeft fl">
                            <asp:DropDownList runat="server" Width="60px" ID="ddlAccountType">
                                <asp:ListItem Value="" Text="全部"></asp:ListItem>
                                <asp:ListItem Text="个人" Value="0" />
                                <asp:ListItem Text="企业" Value="1" />
                            </asp:DropDownList>
                        </div>
                    </div>
                </td>
                  <td>
                    <div class="input">
                        <span class="name">申请时间：</span>
                        <asp:TextBox ID="txtBeginTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtEndTime\')}'})"></asp:TextBox>&nbsp;&nbsp;至&nbsp;&nbsp;&nbsp;<asp:TextBox
                            ID="txtEndTime" runat="server" CssClass="text text-s" onClick="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtBeginTime\')}'})"></asp:TextBox>
                            <asp:HiddenField ID="hdDefaultDate" runat="server" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="input">
                        <span class="name">企业名称：</span>
                        <asp:TextBox ID="txtCompayName" runat="server" CssClass="text"></asp:TextBox>
                    </div>
                </td>
                <td>
                    <div class="input" style="z-index:1;">
                        <span class="name">审核类型：</span>
                        <asp:DropDownList ID="ddlAuditType" runat="server">
                            <asp:ListItem Text="全部" Value=""></asp:ListItem>
                            <asp:ListItem Text="普通审核" Value="普通审核"></asp:ListItem>
                            <asp:ListItem Text="变更审核" Value="变更审核"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                   <div class="input">
                     <span class="name">来源渠道：</span>
                     <asp:DropDownList ID="ddlRegisterType" runat="server">
                       <asp:ListItem Text="全部" Value=""></asp:ListItem>
                       <asp:ListItem Text="自然注册" Value="自然注册"></asp:ListItem>
                       <asp:ListItem Text="推荐人" Value="推荐人"></asp:ListItem>
                     </asp:DropDownList>
                    
                   </div>
                </td>
            </tr>
            <tr class="btns">
              <td colspan="3">
                 <asp:Button ID="btnSearch" runat="server" CssClass="btn class1" Text="查 询" OnClick="btnSearch_Click" />
                    <input type="button" value="清空条件" onclick="ResetSearchOption()" class="btn class2" />
              </td>
            </tr>
        </table>
    </div>
    <div class="column table" id='data-list'>
        <asp:Repeater runat="server" ID="datalist" OnPreRender="AddEmptyTemplate" EnableViewState="False">
            <HeaderTemplate>
                <table id="dataListTable">
                    <tr>
                        <th>
                            用户名
                        </th>
                        <th>
                            企业名称
                        </th>
                        <th>
                            帐号类型
                        </th>
                        <th>
                            申请时间
                        </th>
                        <th>
                          来源渠道
                        </th>
                        <th>
                            审核类型
                        </th>
                        <th>
                            操作
                        </th>
                    </tr>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%#Eval("Account")%>
                    </td>
                    <td>
                        <%#Eval("CompanyName")%>
                    </td>
                    <td>
                        <%#Eval("AccountType")%>
                    </td>
                    <td>
                        <%#Eval("Time")%>
                    </td>
                    <td>
                      <%#Eval("SourceType")%>
                    </td>
                    <td>
                        <%#Eval("AuditType") %>
                    </td>
                    <td>
                        <a href="../CompanyInfoManage/CertificationAudit.aspx?CompanyId=<%#Eval("Id") %>&CompanyType=<%#Eval("CompanyTypeValue") %>&AccountType=<%#Eval("AccountTypeValue") %>&AuditType=<%#Eval("AuditTypeValue") %>">
                            立即审核</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager ID="pager" runat="server" Visible="false" />
    </div>
    </form>
</body>
</html>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<script src="../../../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../../../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        SaveDefaultData();
    });
</script>
