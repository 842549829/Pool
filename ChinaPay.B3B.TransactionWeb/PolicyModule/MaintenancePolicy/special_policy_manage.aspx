<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="special_policy_manage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.special_policy_manage" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register TagPrefix="uc1" TagName="Company_2" Src="~/UserControl/CompanyC.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>运营特殊政策管理</title>
</head>
    <link rel="stylesheet" href="/Styles/icon/fontello.css" />
    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="form1" runat="server" autocomplete="off">
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
                                <span class="name">特殊类型：</span>
                                <asp:DropDownList ID="ddlSpecialType" runat="server">
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">是否有效：</span>
                                <asp:RadioButton ID="radYouxiaoAll" runat="server" Text="所有" Checked="true" GroupName="Youxiao" />
                                <asp:RadioButton ID="radYouxiao" runat="server" Text="有效" GroupName="Youxiao" />
                                <asp:RadioButton ID="radGuoqi" runat="server" Text="无效" GroupName="Youxiao" />
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
                                <span class="name">修改日期：</span>
                                <asp:TextBox ID="txtPubStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({ isShowClear:false,readOnly:true, maxDate: '#F{$dp.$D(\'txtPubEndTime\')||\'2020-10-01\'}' })"></asp:TextBox>
                                -
                                <asp:TextBox ID="txtPubEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({ isShowClear:false,readOnly:true, minDate: '#F{$dp.$D(\'txtPubStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">平台评审：</span>
                                <asp:RadioButton ID="radSuoyou" runat="server" Text="所有" Checked="true" GroupName="radpingtai" />
                                <asp:RadioButton ID="radYijing" runat="server" Text="已审" GroupName="radpingtai" />
                                <asp:RadioButton ID="radNotYijing" runat="server" Text="未审" GroupName="radpingtai" />
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
                                <span class="name">航班日期：</span>
                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false,  readOnly:true, maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })"></asp:TextBox>
                                -
                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({ isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox></div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">产品评审：</span>
                                <asp:RadioButton ID="radAutoAll" runat="server" Text="所有" Checked="true" GroupName="radProduct" />
                                <asp:RadioButton ID="radAutoed" runat="server" Text="已审" GroupName="radProduct" />
                                <asp:RadioButton ID="radNot" runat="server" Text="未审" GroupName="radProduct" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">供应账号：</span>
                                <uc1:Company_2 ID="AgentCompany" runat="server" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">是否挂起：</span>
                                <asp:DropDownList ID="ddlGua" runat="server">
                                    <asp:ListItem Value="99" Text="-请选择-" />
                                    <asp:ListItem Value="1" Text="平台挂起" />
                                    <asp:ListItem Value="2" Text="公司挂起" />
                                    <asp:ListItem Value="0" Text="未挂" />
                                </asp:DropDownList>
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">锁定状态：</span>
                                <asp:RadioButton ID="radLockAll" runat="server" Text="所有" Checked="true" GroupName="radioone" />
                                <asp:RadioButton ID="radLock" runat="server" Text="锁定" GroupName="radioone" />
                                <asp:RadioButton ID="radUnLock" runat="server" Text="未锁" GroupName="radioone" />
                            </div>
                        </td>   
                    </tr>
                    <tr>
                        <td colspan="3" class="btns">
                            <asp:Button ID="btnQuery" CssClass="btn class1" runat="server" Text="查询" OnClick="btnQuery_Click" />
                            <input type="button" id="lock" class="btn class2" value="锁定" />
                            <input type="button" id="unlock" class="btn class2" value="解锁" />
                            <input type="button" value="清空条件" class=" btn class2" onclick="ResetSearchOption();" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div id="data-list" class="table">
        <asp:GridView ID="grv_special" runat="server" CssClass="info" AutoGenerateColumns="false"
            Width="100%" OnRowCommand="grv_specical_RowCommand">
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
                            <%#Eval("Departure")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="到达城市">
                    <ItemTemplate>
                        <span class="Arrival">
                            <%#Eval("Arrival")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="特殊票类型" DataField="SpecialType" />
                <asp:TemplateField HeaderText="排除日期">
                    <ItemTemplate>
                        <span class="DepartureDateFilter">
                            <%#Eval("DepartureDateFilter")%>
                        </span>
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
                <asp:TemplateField HeaderText="航班日期">
                    <ItemTemplate>
                        <%#Eval("DepartureDates")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="产品方" DataField="Opearor" />
                <asp:BoundField HeaderText="产品审核" DataField="Sudit" />
                <asp:BoundField HeaderText="平台审核" DataField="State" />
                <asp:BoundField HeaderText="是否锁定" DataField="Lock" />
                <asp:BoundField HeaderText="是否挂起" DataField="Hang" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='special_policy_info.aspx?id=<%#Eval("id") %>'>查看</a><br />
                        <%#Eval("LockTip")%><br />
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%#Eval("id") %>' CommandName='<%#Eval("SuditName") %>'
                            runat="server"><%#Eval("SuditTip")%></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <div class="box" id="showempty" visible="false" runat="server">
        没有任何符合条件的查询结果</div>
    <asp:HiddenField ID="hidIds" runat="server" />
    <asp:HiddenField ID="hdDefaultDate" runat="server" />
    <a id="divOpcial" style="display: none;" data="{type:'pop',id:'divPolicy'}"></a>
    <div id="divPolicy" class="form layer" style="display: none">
        <h2>
            解锁政策</h2>
        <table>
            <colgroup>
                <col class="w30" />
                <col class="w70" />
            </colgroup>
            <tr>
                <td class="title">
                    请输入解锁原因
                </td>
                <td>
                    <asp:TextBox ID="txtlockReason" TextMode="MultiLine" Height="150px" Width="300px"
                        runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtunlockReason" TextMode="MultiLine" Height="150px" Width="300px"
                        runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <asp:Button ID="btnSavelock" CssClass="btn class1" runat="server" Text="锁&nbsp;&nbsp;&nbsp;定"
                        OnClick="btnSavelock_Click" />
                    <asp:Button ID="btnSaveunlock" CssClass="btn class1" runat="server" Text="解&nbsp;&nbsp;&nbsp;锁"
                        OnClick="btnSaveunlock_Click" />
                    <label class="close btn class2" title="关闭">
                        关闭</label>
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
    </form>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/checkboxchooice.js?20130416" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
    <script src="/Scripts/PolicyModule/policylock.js?20130411" type="text/javascript"></script>
    <%--<script src="/Scripts/FixTable.js" type="text/javascript"></script>--%>
    <script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20130428" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".DepartureDateFilter,.DepartureWeekFilter,").tipTip({ maxWidth: "400px", limitLength: 6 });
            $(".Departure,.Arrival,.Include,.Exclude").tipTip({ limitLength: 6, maxWidth: "300px" });
            $("#txtStartTime").focus(function () {
                WdatePicker({ skin: 'default', maxDate: '#F{$dp.$D(\'txtEndTime\')||\'2020-10-01\'}' })
            });
            $("#txtEndTime").focus(function () {
                WdatePicker({ skin: 'default', minDate: '#F{$dp.$D(\'txtStartTime\')}', maxDate: '2020-10-01' })
            });
            //            if ($("#grv_special tr").length > 0) {
            //                FixTable("grv_special", 15, 100);
            //            }
            SaveDefaultData();
        });
    </script>
</body>
</html>
