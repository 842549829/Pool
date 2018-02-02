<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bargain_defaultpolicy_manage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.bargain_defaultpolicy_manage" %>
    <%@ Register TagPrefix="uc1" TagName="Company_2" Src="~/UserControl/CompanyC.ascx" %>
    <%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>特价默认政策</title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link rel="stylesheet" href="/Styles/public.css?20130301" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />

<body>
    <form id="form1" runat="server" autocomplete="off">
        <h3 class="titleBg">
            特价默认政策</h3>
        <div class="box-a">
            <div class="condition">
                <table>
                    <colgroup>
                        <col class="w35" />
                        <col class="w35" />
                        <col class="w30" />
                    </colgroup>
                    <tbody>
                        <tr>
                            <td>
                                <div class="input">
                                    <span class="name">航空公司：</span>
                                    <asp:DropDownList ID="ddlAirlines" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td>
                             <div class="input">
                               <span class="name">出港省份：</span>
                               <asp:DropDownList ID="ddlProvince" runat="server"></asp:DropDownList>
                             </div>
                            </td>
                            <td>
                              <div class="input">
                                <span class="name">成人出票方：</span>
                                <uc1:Company_2 ID="AdultAgentCompany" runat="server" />
                              </div>
                            </td>
                        </tr>
                        <tr>
                          <td  colspan="3" class="btns">
                           <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn class1" OnClick="btnQuery_Click" />
                                    <input type="button" value="添加" class="btn class1" onclick="javascript:window.location.href='bargain_defaultpolicy_addormodify.aspx'" />
                          </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
            <div class="table" id="data-list">
        <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" 
                    onrowcommand="dataSource_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="航空公司" DataField="Airline" />
                <asp:BoundField HeaderText="出港省份" DataField="Province" />
                <asp:BoundField HeaderText="成人默认出票方" DataField="AdultProviderAbbreviateName" />
                <asp:BoundField HeaderText="成人默认佣金" DataField="AdultCommission" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='bargain_defaultpolicy_addormodify.aspx?airline=<%#Eval("Airline") %>&ProvinceCode=<%#Eval("ProvinceCode") %>'>修改</a>
                        <asp:LinkButton ID="lnkDel" runat="server" CommandName="del" CommandArgument='<%# Eval("Airline")+","+Eval("ProvinceCode")  %>' OnClientClick='return confirm("您确定要删除吗？")'>删除</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    </div>
    <div class="btns">
        <uc:pager runat="server" id="pager" visible="false"></uc:pager>
    </div>

    </form>
</body>
</html>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
<script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
