<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExternalPlatformSetting.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.ExternalPlatformSetting" %>

<%@ Register Src="~/UserControl/CompanyC.ascx" TagName="Company" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        外平台接口设置：</h3>
    <div class="form">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w20" />
                    <col class="w80" />
                </colgroup>
                <tr>
                    <td class="title">
                        接口平台:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPlatform" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        留点:
                    </td>
                    <td>
                        <asp:TextBox ID="txtDeduct" runat="server"></asp:TextBox>%
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        政策差:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRebateBalance" runat="server"></asp:TextBox>%
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        出票方:
                    </td>
                    <td>
                        <uc:Company ID="txtProviderCompany" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        支付方式优先级:
                    </td>
                    <td>
                        <div>
                            <div id="divPayInterface" runat="server">
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        状态:
                    </td>
                    <td>
                        <asp:RadioButton ID="rbnEnable" runat="server" GroupName="status" Checked="true"
                            Text="启用" />
                        <asp:RadioButton ID="rbnDisable" runat="server" GroupName="status" Text="禁用" />
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button CssClass="btn class1" Text="保存" runat="server" ID="btnSave" OnClick="btnSave_Click" />
                        <input type="button" value="返回" class="btn class2" onclick="javascript:window.location.href='ExternalPlatformList.aspx'" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hfdPayInterface" runat="server" />
    <asp:HiddenField ID="hfdCompanyAccount" runat="server" />
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script type="text/javascript" src="/Scripts/widget/common.js?2013117"></script>
<script src="../../Scripts/selector.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $("#btnSave").click(function () {

            if ($.trim($("#ddlPlatform").val()) == "") {
                alert("请选择外平台接口");
                return false;
            }
            var ratePattern = new RegExp(/^[0-9]{1,2}(\.[1-9])?$/);
            if (!ratePattern.test($.trim($("#txtDeduct").val()))) {
                alert("留点格式错误");
                $("#txtDeduct").select();
                return false;
            }
            if (!ratePattern.test($.trim($("#txtRebateBalance").val()))) {
                alert("政策差格式错误");
                $("#txtRebateBalance").select();
                return false;
            }
            if ($("#txtProviderCompany_hidCompanyId").val() == "") {
                alert("请选择出票方");
                return false;
            }
            var str = '';
            for (var i = 0; i < $("#divPayInterface select option:selected[value!='']").size(); i++) {
                str += $("#divPayInterface select option:selected[value!='']").eq(i).val() + '|';
            }
            str = str.substring(0, str.length - 1);
            $("#hfdPayInterface").val(str);
            $("#hfdCompanyAccount").val($("#txtProviderCompany_txtCompanyName").text().substring(0,
            $("#txtProviderCompany_txtCompanyName").text().indexOf('-')));
        });
    })
</script>
