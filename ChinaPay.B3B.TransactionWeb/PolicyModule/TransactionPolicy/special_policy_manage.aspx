<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="special_policy_manage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.special_policy_manage" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户特殊政策管理</title>
</head>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        特殊政策管理</h3>
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
                                <asp:DropDownList ID="ddlAirline" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td> 
                            <div class="input">
                                <span class="name">是否有效：</span>
                                <asp:DropDownList ID="ddlYouxiao" runat="server">
                                    <asp:ListItem Text="-请选择-" Value="99"></asp:ListItem>
                                    <asp:ListItem Text="仅显示有效" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="仅显示过期" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input radio_reset">
                                <span class="name">政策状态：</span>
                                <asp:RadioButton ID="radAuditAll" runat="server" Text="所有" Checked="true" GroupName="radioone" />
                                <asp:RadioButton ID="radAudit" runat="server" Text="审核" GroupName="radioone" />
                                <asp:RadioButton ID="radUnAudit" runat="server" Text="未审" GroupName="radioone" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">出发城市：</span>
                                <uc:City runat="server" ID="txtDeparture"></uc:City>
                            </div>
                        </td> 
                        <td>
                          <div class="input">
                            <span class="name">特殊类型：</span>
                            <asp:DropDownList ID="ddlSpecialType" runat="server"></asp:DropDownList>
                          </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">修改日期：</span>
                                <asp:TextBox ID="txtPubStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false,  readOnly:true, maxDate: '#F{$dp.$D(\'txtPubEndTime\')||\'2020-10-01\'}' })"></asp:TextBox> -
                                <asp:TextBox ID="txtPubEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({ isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtPubStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">到达城市：</span>
                                <uc:City runat="server" ID="txtArrival"></uc:City>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">舱位：</span> 
                                <asp:TextBox ID="txtBunks" runat="server" CssClass="text"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">航班日期：</span>
                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({  readOnly:true, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })"></asp:TextBox> -
                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({  readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="btns" colspan="4">
                            <asp:Button ID="btnQuery" runat="server" CssClass="btn class1" Text="查 询" OnClick="btnQuery_Click" />
                            <asp:Button ID="btnPublish" runat="server" CssClass="btn class2" Text="发 布" OnClick="btnPublish_Click" />
                            <asp:Button ID="btnAudited" runat="server" CssClass="btn class2" Text="审 核" OnClick="btnAudited_Click" />
                            <asp:Button ID="btnUnAudited" runat="server" CssClass="btn class2" Text="取消审核" OnClick="btnUnAudited_Click" />
                            <asp:Button ID="btnDel" runat="server" CssClass="btn class2" Text="删 除" OnClick="btnDel_Click" />
                            <input type="button" value="清空条件" class=" btn class2" onclick="ResetSearchOption();" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_specical" runat="server" CssClass="nfo-table info" AutoGenerateColumns="false"
            OnRowCommand="grv_specical_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <input type="checkbox" class="chkAll" value="" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" class="chkOne" id="chk" runat="server" value='<%#Eval("id") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:BoundField HeaderText="航空公司" DataField="Airline" />
                <asp:TemplateField HeaderText="出发城市">
                    <ItemTemplate>
                        <span class="Departure">
                            <%#Eval("Departure")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="到达城市">
                    <ItemTemplate>
                        <span class="Arrival">
                            <%#Eval("Arrival")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="特殊票类型" DataField="Type" />
                <asp:TemplateField HeaderText="排除日期">
                    <ItemTemplate>
                        <span class="DepartureDateFilter">
                            <%#Eval("DepartureDateFilter")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="适用班期">
                    <ItemTemplate>
                        <span class="DepartureWeekFilter">
                            <%#Eval("DepartureWeekFilter")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="航班限制">
                    <ItemTemplate>
                        <span class="Include">
                            <%#Eval("Include")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="提前天数" DataField="BeforehandDays" />
                <asp:BoundField HeaderText="价格/直减" DataField="PriceInfo" />
                <asp:TemplateField HeaderText="返佣">
                    <ItemTemplate>
                        <span>
                            <%#Eval("Commission")%>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="去程日期">
                    <ItemTemplate>
                        <%#Eval("DepartureDates")%></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="发布者" DataField="Opearor" />
                <asp:BoundField HeaderText="政策状态" DataField="Sudit" />
                <asp:BoundField HeaderText="是否挂起" DataField="Hang" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%#Eval("id") %>' CommandName='<%#Eval("SuditName") %>'
                            runat="server"><%#Eval("SuditTip")%></asp:LinkButton><br />
                        <a href="javascript:ModifyCommission('<%#Eval("id") %>','<%#Eval("Price") %>','<%#Eval("PriceType") %>','<%#Eval("TypeValue") %>','<%#Eval("IsInternal") %>','<%#Eval("IsPeer") %>','<%#Eval("InternalCommission") %>','<%#Eval("SubordinateCommission") %>','<%#Eval("ProfessionCommission") %>','<%#Eval("IsBargainBerths") %>');">
                            修改返佣</a><br />
                        <asp:LinkButton ID="LinkButton2" CommandArgument='<%#Eval("id") %>' CssClass="del_click"
                            CommandName='del' runat="server">删 除</asp:LinkButton><br />
                        <a href='special_policy_edit.aspx?Id=<%#Eval("id") %>&Type=Update'>修改详细</a> <br /><a href='special_policy_edit.aspx?Id=<%#Eval("id") %>&Type=Copy'>
                            政策复制</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <div class="btns">
            <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
        </div>
        <div class="box" id="showempty" visible="false" runat="server">
            没有任何符合条件的查询结果</div>
        <asp:HiddenField ID="hidIds" runat="server" />
        <asp:HiddenField ID="hdDefaultDate" runat="server" />
    </div>
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none">
        <h2>
            特殊政策修改返佣</h2>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w30" />
                <col class="w20" />
                <col class="w30" />
            </colgroup>
            <tr class="priceInfoT">
                <td class="title">
                    <input type="radio" id="radPrice" runat="server" name="pricetype"  value="0" />
                    <label for="radPrice">
                        价格：</label><br />
                    <input type="radio" id="radLapse" runat="server" class="subString" name="pricetype" value="2" />
                    <label for="radLapse" class="subString">
                        直减：</label><br />
                    <input type="radio" id="radCommission" runat="server" style="display:none;" class="subString" name="pricetype" value="3" /> 
                </td>
                <td>
                    <asp:Label ID="lblSign" CssClass="priceInfo" runat="server">￥</asp:Label>
                    <asp:TextBox ID="txtPrice" Width="50px" MaxLength="5" CssClass="text priceInfo" runat="server"></asp:TextBox><br />
                    <asp:TextBox ID="txtLapse" Width="50px" MaxLength="4" CssClass="text discount subString" runat="server"></asp:TextBox><asp:Label
                        ID="lblDiscount" CssClass="discount subString" runat="server">%</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="title subordinate">
                    下级返佣
                </td>
                <td class="subordinate">
                    <asp:TextBox ID="txtSubordinateCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox><asp:Label ID="lblSubordernate" CssClass="sign" runat="server">%</asp:Label>
                </td>
                <td class="title  allowBrotherPurchase">
                    同行返佣
                </td>
                <td class="allowBrotherPurchase">
                    <asp:TextBox ID="txtProfessionCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox><asp:Label ID="lblProffession" CssClass="sign" runat="server">%</asp:Label>
                </td>
            </tr>
            <tr id="commission">
                <td class="title canHaveSubordinate">
                    内部返佣
                </td>
                <td colspan="3" class="canHaveSubordinate">
                    <asp:TextBox ID="txtInternalCommission" Width="50px" MaxLength="4" CssClass="text"
                        runat="server"></asp:TextBox><asp:Label ID="Label1" CssClass="sign" runat="server">%</asp:Label>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="4">
                    <asp:Button ID="btnSave" CssClass="btn class1" runat="server" Text="保&nbsp;&nbsp;&nbsp;存"
                        OnClick="btnSave_Click" />
                    <input class="close btn class2" value="关&nbsp;&nbsp;&nbsp;闭" type="button" />
                </td>
            </tr>
        </table>
    </div>
    
    <a id="divChooice" style="display: none;" data="{type:'pop',id:'divIsAll'}"></a>
    <div id="divIsAll" class="form layer" style="display: none">
        <h2>
            确认选择范围</h2>
        <div style="text-align:center;">
            <input type="button" id="btnAll" class="btn class1" value="选择所有政策" />
            <input type="button" id="btnCurrt" class="btn class1" value="选择当前页政策" />
            <input type="button" id="btnCancel" class="close btn class2" value="取消" />
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hidIsAll" />
    <asp:HiddenField runat="server" ID="hfdSpecialType" />
    <asp:HiddenField runat="server" ID="hfdIsInternal" />
    <asp:HiddenField runat="server" ID="hfdIsPeer" />
    </form>
    <script src="/Scripts/DatePicker/WdatePicker.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205?20121101" type="text/javascript"></script>
    <script src="/Scripts/checkboxchooice.js?20130416" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118?20121101" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/polciy_public.js?20121205" type="text/javascript"></script>
    <script src="/Scripts/jquery.tipTip.minified.js?20121101" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/SpecialPolicy.js?20121101" type="text/javascript"></script>
</body>
</html>
