<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LookUpCompanyInfo.aspx.cs" EnableViewState="false"
    Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.LookUpCompanyInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <h2>公司基础信息</h2>
    <div class="form">
        <table>
            <colgroup>
                <col class="w15" /><col class="w35" /><col class="w15" /><col class="w35" />
            </colgroup>
            <tr>
                <td class="title">用户名：</td>
                <td><asp:Label ID="lblAccountNo" runat="server"></asp:Label></td>
                <td class="title">公司类型：</td>
                <td><asp:Label ID="lblCompanyType" runat="server"></asp:Label></td>
            </tr>
            <tr id="lblIndividual" runat="server">
              <td class="title">真实姓名：</td>
              <td><asp:Label ID="lblTrueName" runat="server"></asp:Label></td>
              <td class="title">身份证号：</td>
              <td><asp:Label ID="lblCertNo" runat="server"></asp:Label></td>
            </tr>
            <tbody  runat="server" id="lblCompany">
                <tr>
                <td class="title">公司名称：</td>
                <td><asp:Label ID="lblCompanyName" runat="server"></asp:Label></td>
                <td class="title">公司简称：</td>
                <td><asp:Label ID="lblComapnyShortName" runat="server"></asp:Label></td>
            </tr>
                <tr>
                <td class="title">公司电话：</td>
                <td><asp:Label ID="lblCompanyPhone" runat="server"></asp:Label></td>
                <td class="title">组织机构：</td>
                <td>
                  <asp:Label ID="lblOrginationCode" runat="server"></asp:Label>
                </td>
            </tr>
            </tbody>
            <tr>
                <td class="title">联系人：</td>
                <td><asp:Label ID="lblLinkman" runat="server"></asp:Label></td>
                <td class="title">联系人手机：</td>
                <td><asp:Label ID="lblLinkmanPhone" runat="server"></asp:Label></td>
            </tr>
            <tbody runat="server" id="tbBuyorOut">
                <tr>
                    <td class="title">负责人：</td>
                    <td><asp:Label ID="lblPrincipal" runat="server"></asp:Label></td>
                    <td class="title">负责人手机：</td>
                    <td><asp:Label ID="lblPrincipalPhone" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td class="title">紧急联系人：</td>
                    <td><asp:Label ID="lblUrgencyLinkman" runat="server"></asp:Label></td>
                    <td class="title">紧急联系人手机：</td>
                    <td><asp:Label ID="lblUrgencyLinkmanPhone" runat="server"></asp:Label></td>
                </tr>
            </tbody>
            <tr>
                <td class="title">所在地：</td>
                <td><asp:Label ID="lblLocation" runat="server"></asp:Label></td>
                <td class="title">地址：</td>
                <td><asp:Label ID="lblAddress" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">QQ：</td>
                <td><asp:Label ID="lblQQ" runat="server"></asp:Label></td>
                <td class="title">Email：</td>
                <td><asp:Label ID="lblEmail" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">邮政编码：</td>
                <td><asp:Label ID="lblPostCode" runat="server"></asp:Label></td>
                <td class="title">传真：</td>
                <td><asp:Label ID="lblFaxes" runat="server"></asp:Label></td>
            </tr>
            <tr id="exceptPurchase" runat="server" visible="false">
               <td class="title">附件：</td>
               <td>
                 <a href="#" id="bussinessLicense" runat="server">营业执照</a>&nbsp;&nbsp;&nbsp;&nbsp;
                 <a href="#" id="certNo" runat="server">身份证</a>&nbsp;&nbsp;&nbsp;&nbsp;
                 <a href="#" id="iata" runat="server">IATA</a>
               </td>
               <td class="title">使用期限：</td>
                <td id="tdCompanyPhone0">
                     <asp:Label ID="lblBeginDeadline" runat="server"></asp:Label>至<asp:Label ID="lblEndDeadline" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title">开户时间</td>
                <td id="registerTime">
                    <asp:Label runat="server" ID="lblRegisterTime"></asp:Label>
                </td>
                <td class="title auditTime">审核时间</td>
                <td class="auditTime">
                    <asp:Label runat="server" ID="lblAudit"></asp:Label>
                </td>
            </tr>
            <tr>
              <td class="title">是否已启用数据接口：</td>
              <td>
                <asp:Label ID="lblIsOpenExternalInterface" runat="server"></asp:Label>
              </td>
              <td class="title">推广人</td>
               <td>
                  <asp:Label ID="lblTuiguang" runat="server"></asp:Label>
              </td>
            </tr>
            <tr runat="server" id="fixedPhone" visible="false">
              <td class="title">固定电话：</td>
              <td><asp:Label ID="lblFixedPhone" runat="server"></asp:Label></td>
            </tr>
        </table>
    </div>
    <!--Office-->
    <h2 id="hOffice" runat="server"></h2>
    <div class="column table" runat="server" id="divOffice"></div>
    <div class="column table" runat="server" id="divPerson"></div>
    <div class="form">
        <h2>账户信息</h2>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tbody>
                <tr>
                    <td class="title">付款账号：</td>
                    <td id="tdpay"><asp:Label ID="lblPayAccount" runat="server"></asp:Label></td>
                    <td class="title put">收款账号：</td>
                    <td class="put"><asp:Label ID="lblPutAccount" runat="server"></asp:Label></td>
                </tr>
            </tbody>
            <tr>
                <td colspan="4"></td>
            </tr>
            <tbody>
                <tr>
                    <td class="title">默认出发城市：</td>
                    <td><asp:Label ID="lblDeparture" runat="server"></asp:Label></td>
                    <td class="title">默认到达城市：</td>
                    <td><asp:Label ID="lblArrival" runat="server"></asp:Label></td>
                </tr>
            </tbody>
            <tbody runat="server" id="tbWorkTime">
                <tr><td colspan="4"><h2>工作时间</h2></td></tr>
                <tr>
                    <td class="title">工作日：</td>
                    <td>
                        <asp:Label ID="lblWorkdayWorkStart" runat="server"></asp:Label>至<asp:Label ID="lblWorkdayWorkEnd" runat="server"></asp:Label>
                    </td>
                    <td class="title">周末日：</td>
                    <td>
                        <asp:Label ID="lblRestdayWorkStart" runat="server"></asp:Label>至<asp:Label ID="lblRestdayWorkEnd" runat="server"></asp:Label>
                    </td>
                </tr>
            </tbody>
            <tbody runat="server" id="tbRefundTime">
                <tr><td colspan="4"><h2>废票时间</h2></td></tr>
                <tr>
                    <td class="title">工作日：</td>
                    <td>
                        <asp:Label ID="lblWorkdayRefundStart" runat="server"></asp:Label>至<asp:Label ID="lblWorkdayRefundEnd" runat="server"></asp:Label>
                    </td>
                    <td class="title">周末日：</td>
                    <td>
                        <asp:Label ID="lblRestdayRefundStart" runat="server"></asp:Label>至<asp:Label ID="lblRestdayRefundEnd" runat="server"></asp:Label>
                    </td>
                </tr>
            </tbody>
            <tbody runat="server" id="tbProviderCompany">
                <tr><td colspan="4"><h2>公司信息</h2></td></tr>
                <tr>
                    <td class="title">允许BSP自动出票：</td>
                    <td><asp:CheckBox ID="chkAutoPrintBSP" runat="server" Enabled="false"/></td>
                    <td class="title">锁定政策累积退废票数：</td>
                    <td><asp:Label ID="lblRefundCountLimit" runat="server"></asp:Label>张</td>
                </tr>
                <tr>
                    <td class="title">允许B2B自动出票：</td>
                    <td><asp:CheckBox ID="chkAutoPrintB2B" runat="server" Enabled="false" /></td>
                    <td class="title">自愿退票时限：</td>
                    <td><asp:Label ID="lblRefundTimeLimit" runat="server"></asp:Label>小时</td>
                </tr>
                <tr>
                    <td class="title">是否可发布VIP政策：</td>
                    <td><asp:CheckBox ID="chkCanReleaseVip" runat="server" Enabled="false" /></td>
                    <td class="title">全退时限：</td>
                    <td><asp:Label ID="lblFullRefundTimeLimit" runat="server"></asp:Label>小时</td>
                </tr>
                <tr>
                    <td class="title">退款财务审核：</td>
                    <td><asp:CheckBox ID="chkRefundNeedAudit" runat="server" Enabled="false" /></td>
                    <td class="title">同行交易费率：</td>
                    <td><asp:Label ID="lblProfessionRate" runat="server"></asp:Label>‰</td>
                </tr>
                <tr>
                    <td class="title">可开内部机构：</td>
                    <td><asp:CheckBox ID="chkCanHaveSubordinate" runat="server" Enabled="false"/></td>
                    <td class="title">下级交易费率：</td>
                    <td><asp:Label ID="lblSubordinateRate" runat="server"></asp:Label>‰</td>
                </tr>
                <tr>
                    <td class="title">平台自动审核政策：</td>
                    <td>
                        <asp:CheckBox ID="chkAutoPlatformAudit1" runat="server" Enabled="false" />
                    </td>
               <%--     <td class="title">特殊票交易费率：</td>
                    <td><asp:Label ID="lblSpecialRate" runat="server"></asp:Label>‰</td>--%>
                    <td class="title">默认出票OFFICE：</td>
                    <td>
                        <asp:Label ID="lblOffice" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="title">允许同行采购：</td>
                    <td><asp:CheckBox ID="chkAllowBrotherPurchase" runat="server" Enabled="false"/></td>
                    <td class="title">单程控位产品：</td>
                    <td>
                        <asp:CheckBox ID="chkSingleness" runat="server" Enabled="false" />
                        <span class="specialProduct"><asp:Label ID="lblSingleness" runat="server"></asp:Label>‰</span>
                    </td>
                </tr>
                <!--特殊产品参数-->
                <tr>
                    
                    <td class="title">散冲团产品：</td>
                    <td>
                        <asp:CheckBox ID="chkDisperse" runat="server" Enabled="false" />
                         <span class="specialProduct"><asp:Label ID="lblDisperse" runat="server"></asp:Label>‰</span>
                    </td>
                    <td class="title">免票产品：</td>
                <td>
                    <asp:CheckBox ID="chkCostFree" runat="server" Enabled="false" />
                    <span class="specialProduct"><asp:Label runat="server" ID="lblCostFree"></asp:Label>‰</span>
                </td>
                    </tr>
                <tr>
                
                    <td class="title">集团票产品：</td>
                    <td>
                    <asp:CheckBox ID="chkBloc" runat="server" Enabled="false" />
                    <span class="specialProduct"><asp:Label ID="lblBloc" runat="server"></asp:Label>‰</span>
                    </td>
                     <td class="title">商旅卡产品：</td>
                    <td>
                    <asp:CheckBox ID="chkBusiness" runat="server" Enabled="false" />
                    <span class="specialProduct"><asp:Label ID="lblBusiness" runat="server"></asp:Label>‰</span>
                    </td>
                </tr>
                <tr>
                    <td class="title">其他特殊产品：</td>
                    <td>
                        <asp:CheckBox ID="chkOtherSpecial" runat="server" Enabled="false" />
                        <span class="specialProduct"><asp:Label ID="lblOtherSpecialRate" runat="server"></asp:Label>‰</span>
                    </td>
                    <td class="title">低打高返特殊产品：</td>
                    <td>
                        <asp:CheckBox ID="chkLowToHighSpecial" runat="server" Enabled="false" />
                        <span class="specialProduct"><asp:Label ID="lblLowToHighSpecialRate" runat="server"></asp:Label>‰</span>
                    </td>
                </tr>
                <tr>
                    <td class="title">儿童返点：</td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkRebateForChild" runat="server" Text="儿童返点" Enabled="false" />
                        <asp:Label ID="lblRebateForChild" runat="server"></asp:Label>
                        <asp:CheckBoxList ID="chklAirlineForChild" runat="server" Enabled="false" RepeatColumns="12" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="title">默认返点：</td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkRebateForDefault" runat="server" Text="默认返点" Enabled="false" />
                        <asp:Label ID="lblRebateForDefault" runat="server"></asp:Label>
                        <asp:CheckBoxList ID="chkAirlineForDefault" runat="server" Enabled="false" RepeatColumns="12" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
                    </td>
                </tr>
            </tbody>
            <tbody id="tbSupplierCompany" runat="server">
                <tr><td colspan="4"><h2>公司信息</h2></td></tr>
                <tr>
                    <%--<td class="title">特殊票交易费率：</td>
                    <td><asp:Label ID="lblSpecialRate1" runat="server"></asp:Label>‰</td>--%>
                    <td class="title">平台自动审核政策：</td>
                    <td colspan="3"><asp:CheckBox ID="chkAutoPlatformAudit" runat="server" Enabled="false" /></td>
                </tr>
                <tr>
                    <td class="title">单程控位产品：</td>
                    <td>
                        <asp:CheckBox ID="chkSingleness1" runat="server" Enabled="false" />
                        <span class="specialProduct"><asp:Label ID="lblSingleness1" runat="server"></asp:Label>‰</span>
                    </td>
                    <td class="title">散冲团产品：</td>
                    <td>
                        <asp:CheckBox ID="chkDisperse1" runat="server" Enabled="false"/>
                         <span class="specialProduct"><asp:Label ID="lblDisperse1" runat="server"></asp:Label>‰</span>
                    </td>
                    </tr>
                <tr>
               <%-- <td class="title">免票产品：</td>
                <td>
                    <asp:CheckBox ID="chkCostFree1" runat="server" Enabled="false" />
                    <span class="specialProduct"><asp:Label runat="server" ID="lblCostFree1"></asp:Label>‰</span>
                </td>--%>
                    <td class="title">集团票产品：</td>
                    <td>
                    <asp:CheckBox ID="chkBloc1" runat="server" Enabled="false" />
                    <span class="specialProduct"><asp:Label ID="lblBloc1" runat="server"></asp:Label>‰</span>
                    </td>
                    <td class="title">商旅卡产品：</td>
                    <td colspan="3">
                    <asp:CheckBox ID="chkBusiness1" runat="server" Enabled="false" />
                    <span class="specialProduct"><asp:Label ID="lblBusiness1" runat="server"></asp:Label>‰</span>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="btns">
        <input type="button" value="返回" class="btn class2" runat="server" id="btnGoBack" />
    </div>
    <input type="hidden" runat="server" id="hidType" />
    <asp:HiddenField ID="hfdBussniess" runat="server" />
    <asp:HiddenField ID="hfdCertNo" runat="server" />
    <asp:HiddenField ID="hfdIATA" runat="server" />
    <asp:HiddenField ID="hidEmployeeNo" runat="server" />
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divZhiding'}"></a>
    <div id="divZhiding" class="form layer" style="display: none">
        <h2 id="important">
            操作提示</h2>
            <h4 class="obvious1">该操作将会使被推广用户的数据发生指向变更，变更后该用户将成为该员工账号下的推广用户</h4>
            <br />
            <table>
                <tr>
                    <td>员工账号 ：</td>
                    <td><input type="text" id="txtEmployeeNo" onkeyup="SelectItem(this,'-');"  runat="server" class="text text-s fl"/>
                        <asp:DropDownList runat="server" ID="selEmployee" CssClass="ctag"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                         <asp:Button ID="btnSave" CssClass="btn class1" runat="server" 
                             Text="确&nbsp;&nbsp;&nbsp;定" onclick="btnSave_Click" />
                         <input class="close btn class2" value="取&nbsp;&nbsp;&nbsp;消" type="button" />
                    </td>
                </tr>
            </table>
    </div>
    </form>
    <script src="/Scripts/selector.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btnSave").click(function () {
                if ($("#selEmployee").val() == "") {
                    alert("请输入员工账号！");
                    return false;
                }
                $("#hidEmployeeNo").val($("#selEmployee").val());
                $("#txtEmployeeNo").val("");
                $("#txtEmployeeNo").keyup();
                return true;
            });
            if ($("#divPayAccount").html() != "") {
                $("#hAccount").show();
            }
            if ($("#hidType").val() == "Supplier") {
                $(".tdCompanyPhone").remove();
                $("#tdCompanyPhone0").attr("colspan", 3);
            } else if ($("#hidType").val() == "Purchaser") {
                $(".put,.auditTime").remove();
                $("#tdpay,#registerTime").attr("colspan", 3);
            }
            $("#bussinessLicense").click(function () {
                window.open($("#hfdBussniess").val());
            });
            $("#certNo").click(function () {
                window.open($("#hfdCertNo").val());
            });
            $("#iata").click(function () {
                window.open($("#hfdIATA").val());
            });

            $(".specialProduct").each(function () {
                var self = $(this); var checkbox = self.prev().children(":checkbox");
                if (checkbox.is(":checked")) { self.show(); } else { self.hide(); }
            });
        });
        function zhiding() {
            $("#divOpcial").click();
            $("#divZhiding").css("top", "150px");
        }
    </script>
</body>
</html>
