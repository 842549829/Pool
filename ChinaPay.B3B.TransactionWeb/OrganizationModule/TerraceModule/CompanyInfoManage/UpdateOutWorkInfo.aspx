<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateOutWorkInfo.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.TerraceModule.CompanyInfoManage.UpdateOutWorkInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ul class="navType1" id="sel" runat="server">
        <li><a href="javascript:;" class="navType1Selected" id="divCompanyInfo">公司信息</a></li>
        <li><a href="javascript:" id="divpay">账户信息</a></li>
        <li><a href="javascript:;" id="divOffices">OFFICE号设置</a></li>
    </ul>
    <div class="divCompanyWorkInfo">
        <h2>公司信息</h2>
        <%--使用期限--%>
        <div class="divCompanyInfo ">
            <div class="col">
                
            </div>
            <div class="form">
                <table>
                    <colgroup>
                        <col class="w20" />
                        <col class="w30" />
                        <col class="w20" />
                        <col class="w40" />
                    </colgroup>
                    <tr>
                        <td class="title">使用期限</td>
                        <td colspan="3"><asp:TextBox ID="txtBeginTime" runat="server" CssClass="datepicker datefrom btn class3"
                    onClick="WdatePicker({isShowClear:false,maxDate: '#F{$dp.$D(\'txtEndTime\')}'})"></asp:TextBox>至
                <asp:TextBox ID="txtEndTime" runat="server" CssClass="datepicker datefrom btn class3"
                    onClick="WdatePicker({isShowClear:false,minDate: '#F{$dp.$D(\'txtBeginTime\')}'})"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="title">
                            允许BSP自动出票
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAllowBSPTicket" runat="server" />
                        </td>
                        <td class="title">
                            锁定政策累积退废票
                        </td>
                        <td>
                            <asp:TextBox ID="txtLockTicket" runat="server" CssClass="text" MaxLength="5"></asp:TextBox>张
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            允许B2B自动出票
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAllowB2BTicket" runat="server" />
                        </td>
                        <td class="title">
                            自愿退票限时
                        </td>
                        <td>
                            <asp:TextBox ID="txtVoluntaryRefundsLimit" runat="server" CssClass="text" MaxLength="5"></asp:TextBox>
                            小时
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            可开内部机构
                        </td>
                        <td>
                            <asp:CheckBox ID="chkInternalOrganization" runat="server" />
                        </td>
                        <%--<td class="title">供应商自己取消PNR</td>
                <td>
                    <asp:CheckBox ID="chkPNR" runat="server" />
                </td>--%>
                        <td class="title">
                            全退限时
                        </td>
                        <td>
                            <asp:TextBox ID="txtAllRefundsLimit" runat="server" CssClass="text" MaxLength="5"></asp:TextBox>小时
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            是否可以发布VIP政策
                        </td>
                        <td>
                            <asp:CheckBox ID="chkVIP" runat="server" />
                        </td>
                        <td class="title">
                            同行交易费率
                        </td>
                        <td>
                            <asp:TextBox ID="txtPeerTradingRate" runat="server" CssClass="text"></asp:TextBox>‰
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            退款财务审核
                        </td>
                        <td>
                            <asp:CheckBox ID="chkRefundFinancialAudit" runat="server" Enabled="false" />
                        </td>
                        <td class="title">
                            下级交易费率
                        </td>
                        <td>
                            <asp:TextBox ID="txtLowerRates" runat="server" CssClass="text"></asp:TextBox>‰
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="height: 30px">
                            平台自动审核政策
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAutomaticAuditPolicy" runat="server" />
                        </td>
                      <%--  <td class="title" style="height: 30px">
                            特殊票交易费率
                        </td>
                        <td class="style1">
                            <asp:TextBox ID="txtSpecialRates" runat="server" CssClass="text"></asp:TextBox>‰
                        </td>--%>
                         <td class="title">
                            默认出票OFFICE
                        </td>
                        <td>
                            <asp:Label ID="lblDefaultOffice" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title" style="height: 30px">
                            允许同行采购
                        </td>
                        <td>
                            <asp:CheckBox ID="chkAllowBrotherPurchase" runat="server" />
                        </td>
                        <td class="title">单程控位产品</td>
                        <td>
                            <asp:CheckBox ID="chkSingleness" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtSingleness" CssClass="text"></asp:TextBox>‰</span>
                        </td>
                    </tr>
                    <!--特殊产品参数-->
                    <tr>
                       
                        <td class="title">散冲团产品</td>
                        <td>
                            <asp:CheckBox ID="chkDisperse" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtDisperse" CssClass="text"></asp:TextBox>‰</span>
                        </td>
                        <td class="title">免票产品</td>
                        <td>
                            <asp:CheckBox ID="chkCostFree" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtCostFree" CssClass="text"></asp:TextBox>‰</span>
                        </td>
                     </tr>
                     <tr>
                        
                         <td class="title">集团票产品</td>
                         <td>
                            <asp:CheckBox ID="chkBloc" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtBloc" CssClass="text"></asp:TextBox>‰</span>
                         </td>
                          <td class="title">商旅卡产品</td>
                         <td>
                            <asp:CheckBox ID="chkBusiness" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtBusiness" CssClass="text"></asp:TextBox>‰</span>
                         </td>
                     </tr>
                    <tr>
                        <td class="title">其他特殊产品</td>
                        <td>
                            <asp:CheckBox ID="chkOtherSpecial" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtOtherSpecialRate" CssClass="text"></asp:TextBox>‰</span>
                        </td>
                        <td class="title">低打高返产品</td>
                        <td>
                            <asp:CheckBox ID="chkLowToHigh" runat="server" />
                            <span class="specialProduct"><asp:TextBox runat="server" ID="txtLowToHighRate" CssClass="text"></asp:TextBox>‰</span>
                        </td>
                     </tr>
                    <tr>
                        <td class="title">
                            儿童票返点
                        </td>
                        <td>
                            <span id="lblChildrens" runat="server">
                                <asp:CheckBox runat="server" ID="chkChildren" Text="儿童返点" Enabled="false" />
                                <span id="lblchildren">
                                    <asp:Label runat="server" ID="lblChildren"></asp:Label>
                                    <i class="must">%</i>
                                    <asp:CheckBoxList ID="chklChildren" runat="server" RepeatColumns="12" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" Enabled="false">
                                    </asp:CheckBoxList>
                                </span></span>
                        </td>
                        <td class="title">信誉评级</td>
                        <td>
                            <asp:DropDownList runat="server" ID="dropCreditworthiness">
                                <asp:ListItem Value="0">0&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="0.5">0.5星</asp:ListItem>
                                <asp:ListItem Value="1">1&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="1.5">1.5星</asp:ListItem>
                                <asp:ListItem Value="2">2&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="2.5">2.5星</asp:ListItem>
                                <asp:ListItem Value="3">3&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="3.5">3.5星</asp:ListItem>
                                <asp:ListItem Value="4">4&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                                <asp:ListItem Value="4.5">4.5星</asp:ListItem>
                                <asp:ListItem Value="5">5&nbsp;&nbsp;&nbsp;星</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            默认返点
                        </td>
                        <td colspan="3">
                            <span id="lblDefaultRebate" runat="server">
                                <asp:CheckBox runat="server" ID="chkDefault" Text="默认返点" Enabled="false" />
                                <span id="lblDefault">
                                    <asp:Label runat="server" ID="lblRebateForDefault"></asp:Label>
                                    <i class="must">%</i>
                                    <asp:CheckBoxList ID="chkAirlineForDefault" runat="server" RepeatColumns="12" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" Enabled="false">
                                    </asp:CheckBoxList>
                                </span></span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="btns">
                <asp:Button CssClass="btn class1" ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" />
                <input type="button" value="返回" class="btn class2" onclick="window.location.href='./CompanyList.aspx?Search=Back'" />
            </div>
        </div>
        <%--Office--%>
        <div id="divProviderOffice" class="divOffices" runat="server">
            <div class="table" runat="server" id="divOffice"></div>
            <div class="btns" style="height:50px;">
                <div  style="margin:10px 10px 10px 10%;width:60%;background-color: #6C7493;height:30px;line-height:30px;color:#fff;">
                    <span class="name">新增</span>
                    <asp:TextBox CssClass="text" runat="server" ID="txtOffice"></asp:TextBox>
                    <input type="checkbox"  value="" id="chkAuthorization"/><label for="chkAuthorization">编码需要授权出票</label>
                    <input type="button" id="btnSaveOffice" class="btn class1" value="保&nbsp;&nbsp;存" />
                </div>
          </div>
        </div>
        <%--账户信息--%>
        <div class="form divpay">
        <table>
            <colgroup>
                <col class="w20"/>
                <col class="w30"/>
                <col class="w20"/>
                <col class="w40"/>
            </colgroup>
            <tr>
                <td class="title">付款账号</td>
                <td>
                    <asp:TextBox ID="txtPayment" runat="server" CssClass="text" disabled="disabled"></asp:TextBox>&nbsp;
                    <span id="lblPayment" runat="server" class="obvious" style="display:block;"></span>
                    <input runat="server" type="button" value="修改" class="btn class1" id="btnPayment"/>
                </td>
                <td class="title">收款账号</td>
                <td>
                    <asp:TextBox ID="txtReceiving" runat="server" CssClass="text" disabled="disabled"></asp:TextBox>&nbsp;
                    <span id="lblReceiving" runat="server" class="obvious" style="display:block;"></span>
                    <input runat="server" type="button" value="修改" class="btn class1" id="btnReceiving" />
                </td>
            </tr>
        </table>
    </div>
    </div>
    <asp:HiddenField runat="server" ID="hidId" />
    </form>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/Account/UpdateAccount.js" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/RoleModule/OfficeNumberManage.js" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/TerraceModule/CompanyWork.js?2012201112" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/TerraceModule/CompanyOption.js" type="text/javascript"></script>
</body>
</html>
