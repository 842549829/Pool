<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="AgentMaintain.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.RoleModule.CompanyInfoMaintain.AgentMaintain" %>

<%@ Register src="../../../UserControl/Airport.ascx" TagName="Airport" TagPrefix="uc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>出票方基础信息维护</title>
    <link href="../../../Styles/core.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/form.css?20121118" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <div class="form">
        <form id="form1" runat="server">
        <h2>公司基础信息</h2>
        <table>
            <colgroup>
                <col class="w15" />
                <col class="w35" />
                <col class="w15" />
                <col class="w35" />
            </colgroup>
            <tr>
                <td class="title">
                    用户名
                </td>
                <td>
                    <asp:Label ID="lblAccountNo" runat="server"></asp:Label>
                </td>
                <td class="title">
                    公司类型
                </td>
                <td>
                    <asp:Label ID="lblCompanyType" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司名称</td>
                <td>
                    <asp:Label ID="lblCompanyName" runat="server"></asp:Label>
                </td>
                <td class="title">
                    公司简称 
                </td>
                <td>
                    <asp:Label ID="lblCompanyShortName" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    所在地
                </td>
                <td>
                    <asp:Label ID="lblAddress" runat="server"></asp:Label>
                </td>
                <td class="title">
                    使用期限
                </td>
                <td>
                    <asp:Label ID="lblBeginDeadline" runat="server" ></asp:Label>至
                    <asp:Label ID="lblEndDeadline" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司地址&nbsp;
                </td>
                <td>
                    <asp:Label ID="lblCompanyAddress" runat="server"></asp:Label>
                </td>
                <td class="title">
                    邮政编码
                    </td>
                <td>
                    <asp:TextBox ID="txtPostCode" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblPostCode"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    公司电话
                </td>
                <td>
                    <asp:TextBox ID="txtCompanyPhone" runat="server" CssClass="text"></asp:TextBox>
                    <span class="tips-txt" id="lblCompanyPhone" runat="server"></span>
                </td>
                <td class="title">
                    传真
                    </td>
                <td>
                    <asp:TextBox ID="txtFaxes" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblFaxes" class="tips-txt"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    负责人
                </td>
                <td>
                    <asp:Label ID="lblPrincipal" runat="server"></asp:Label>
                </td>
                <td class="title">
                    负责手机
                </td>
                <td>
                    <asp:Label ID="lblPrincipalPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    联系人 
                </td>
                <td>
                    <asp:Label ID="lblLinkman" runat="server"></asp:Label>
                </td>
                <td class="title">
                    联系人手机
                </td>
                <td>
                    <asp:Label ID="lblLinkManPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    紧急联系人 
                </td>
                <td>
                    <asp:Label ID="lblUrgencyLinkMan" runat="server"></asp:Label>
                </td>
                <td class="title">
                    紧急联系人手机
                </td>
                <td>
                    <asp:Label ID="lblUrgencyLinkManPhone" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">
                    E_Mail
                    </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblEmail"></span>
                </td>
                <td class="title">
                    MSN
                </td>
                <td>
                    <asp:TextBox ID="txtMSN" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblMSN"></span>
                </td>
            </tr>
            <tr>
                <td class="title">
                    QQ
                </td>
                <td>
                    <asp:TextBox ID="txtQQ" runat="server" CssClass="text"></asp:TextBox>
                    <span id="lblQQ"></span>
                </td>
                <td class="title">
                </td>
                <td></td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnSvaeCompanyInfo" runat="server" Text="保存" CssClass="btn class1" onclick="btnSvaeCompanyInfo_Click" />
                </td>
            </tr>
            <tr class="table column">
                <td colspan="4">
                    <h2>OFFICE</h2>
                    <div runat="server" id="divOffice"></div>
                </td>
            </tr>
            <tr>
                <td colspan="4"  style="border-top-style:hidden; border-top-width:0px;">
                    <h2>公司工作信息</h2>
                </td>
            </tr>
            <tr>
                <td class="title">默认出发城市</td>
                <td>
                    <uc:Airport ID="Departure" runat="server"  />
                </td>
                <td class="title">默认到达城市</td>
                <td>
                    <uc:Airport ID="Arrival" runat="server"/>    
                </td>
            </tr>
            <tr>
                <td class="title">
                    退款财务审核
                </td>
                <td>
                    <asp:CheckBox ID="chkRefundFinancialAudit" runat="server"/>
                </td>
                <td class="title">默认出票OFFICE</td>
                <td>
                    <asp:DropDownList ID="ddlOffice" runat="server" CssClass="text dropdown"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="title">
                    成人可出票航空公司
                </td>
                <td colspan="3">
                    <asp:CheckBoxList ID="chklAirlines" runat="server" RepeatColumns="15" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="false">
                    </asp:CheckBoxList>
                </td>
            </tr>
            <tr>
                <td class="title">
                    儿童返佣
                </td>
                <td colspan="3">
                    <input type="checkbox" id="chkChildern" runat="server"/><label for="chkChildern">儿童返点</label><br />
                    <span id="lblChildern">
                        <labe>儿童返点: </label>
                        <asp:TextBox ID="txtCholdrenDeduction" runat="server" CssClass="text"></asp:TextBox>
                        <i class="must">%</i><br /><i class="must">可发布儿童政策的航空公司：</i><br />
                        <asp:CheckBoxList ID="chklCholdrenDeduction" runat="server" RepeatColumns="15" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
                    </span>
                </td>
            </tr>
            <tr class="btns">
            <td colspan="4">
                <asp:Button ID="btnSaveChilder" runat="server" Text="保存" CssClass="btn class1" onclick="btnSaveChilder_Click"/>
            </td>
        </tr>
        </table>
        <div class="table" id="divPerson">
            <table>
                <colgroup><col class='w25' /><col class='w25' /><col class='w25' /><col class='w25' /></colgroup>
                <tr><th>负责方向</th><th>负责人</th><th>手机</th><th>QQ</th></tr>
                <tr>
                    <td>出票</td>
                    <td><asp:TextBox ID="txtDrawerPerson" runat="server" CssClass="text"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtDrawerCellPhone" runat="server" CssClass="text"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtDrawerQQ" runat="server" CssClass="text"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>退票</td>
                    <td><asp:TextBox ID="txtRetreatPerson" runat="server" CssClass="text"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtRetreatCellPhone" runat="server" CssClass="text"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtRetreatQQ" runat="server" CssClass="text"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>废票</td>
                    <td><asp:TextBox ID="txtWastePerson" runat="server" CssClass="text"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtWasteCellPhone" runat="server" CssClass="text"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtWasteQQ" runat="server" CssClass="text"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>改期</td>
                    <td><asp:TextBox ID="txtReschedulingPerson" runat="server" CssClass="text"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtReschedulingCellPhoen" runat="server" CssClass="text"></asp:TextBox></td>
                    <td><asp:TextBox ID="txtReschedulingQQ" runat="server" CssClass="text"></asp:TextBox></td>
                </tr>
            </table>
        </div>
        <div class="btns">
                <asp:Button ID="btnSave" runat="server" CssClass="btn class1" Text="保存" onclick="btnSave_Click" />
        </div>
        </form>
    </div>
    <script src="../../../Scripts/selector.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/FixityInformation.js" type="text/javascript"></script>
    <script src="../../../Scripts/OrganizationModule/RoleModule/CompanyInfoMaintain.js" type="text/javascript"></script>
    <script type="text/javascript">
        window.onload = function () {
            var childern = document.getElementById("chkChildern"), lblChildern = document.getElementById("lblChildern");
            ChilderChecked(childern, lblChildern);
            childern.onclick = function () { ChilderChecked(childern, lblChildern);}
        }
        function ChilderChecked(childern, lblChildern) { childern.checked ? lblChildern.style.display = "block" : lblChildern.style.display = "none";}
    </script>
</body>
</html>
