<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreaRelateList.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.AreaRelateList" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
    <link rel="stylesheet" href="/Styles/public.css?20121118" />
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="query" runat="server">
        <h3 class="titleBg">
            销售区域关联设置</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col />
                    <col />
                    <col class="w20" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">销售区域：</span>
                            <asp:TextBox ID="txtAreaName" runat="server" CssClass="text"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">省份名称：</span>
                            <asp:TextBox ID="txtPronvice" runat="server" CssClass="text"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn class1" OnClick="btnQuery_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table" id="dataSource" runat="server">
        <asp:Repeater runat="server" ID="dataList" EnableViewState="false">
            <HeaderTemplate>
                <table>
                    <thead>
                        <tr>
                            <th>
                                省份名称
                            </th>
                            <th>
                                销售区域
                            </th>
                            <th>
                                操作
                            </th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <%# Eval("ProvinceName")%>
                    </td>
                    <td>
                        <%# Eval("AreaName")%>
                    </td>
                    <td>
                        <a href="MarketingAreaRelationSet.aspx?provinceCode=<%# Eval("ProvinceCode") %>"
                            class="obvious-a">设置关联</a>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                </tbody> </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
    <div class="box" runat="server" visible="false"  id="emptyDataInfo">
            没有任何符合条件的查询结果
        </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnQuery").click(function () {
            if ($.trim($("#txtAreaName").val()).length > 25) {
                alert("区域名称位数不能超过25位！");
                $("#txtAreaName").select();
                return false;
            }
            if ($.trim($("#txtPronvice").val()).length > 20) {
                alert("省份名称位数不能超过20位！");
                $("#txtPronvice").select();
                return false;
            }
        });
    })
</script>
