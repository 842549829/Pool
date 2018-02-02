<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="low_price_policy_manage.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.MaintenancePolicy.low_price_policy_manage" %>

<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/Airport.ascx" TagName="City" TagPrefix="uc" %>
<%@ Register TagPrefix="uc1" TagName="Company_2" Src="~/UserControl/CompanyC.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>运营方特价政策管理</title>
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>    <link href="/Styles/tipTip.css" rel="stylesheet" type="text/css" />

<body>
    <form id="form1" runat="server" autocomplete="off">
    <h3 class="titleBg">
        特价政策管理</h3>
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
                                <span class="name">行程类型：</span>
                                <asp:DropDownList ID="ddlVoyage" runat="server">
                                    <asp:ListItem Value="" Text="-请选择-" />
                                    <asp:ListItem Value="1" Text="单程" />
                                    <asp:ListItem Value="2" Text="往返" />
                                    <asp:ListItem Value="8" Text="中转联程" />
                                </asp:DropDownList>
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
                                <asp:TextBox ID="txtPubStartTime" runat="server" CssClass="text text-s" onclick="WdatePicker({isShowClear:false,  readOnly:true, maxDate: '#F{$dp.$D(\'txtPubEndTime\')||\'2020-10-01\'}' })"></asp:TextBox>
                                -
                                <asp:TextBox ID="txtPubEndTime" runat="server" CssClass="text text-s" onclick="WdatePicker({ isShowClear:false, readOnly:true, minDate: '#F{$dp.$D(\'txtPubStartTime\')}', maxDate: '2020-10-01' })"></asp:TextBox>
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
                                <span class="name">中转城市：</span>
                                <uc:City ID="txtTransit" runat="server" />
                            </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">航班日期：</span>
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="text text-s" onclick="WdatePicker({   readOnly:true, maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' })"></asp:TextBox>
                                -
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="text text-s" onclick="WdatePicker({   readOnly:true, minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate: '2020-10-01' })"></asp:TextBox></div>
                            <%--<span class="input"><span class="name">内部返佣：</span>
                                <asp:TextBox ID="txtInternalCommissionStart" Style="width: 30px" runat="server" CssClass="text"></asp:TextBox>
                                % -
                                <asp:TextBox ID="txtInternalCommissionEnd" Style="width: 30px" runat="server" CssClass="text"></asp:TextBox>
                                % </span>--%>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">锁定状态：</span>
                                <asp:RadioButton ID="radLockAll" runat="server" Text="所有" Checked="true" GroupName="radlock" />
                                <asp:RadioButton ID="radLock" runat="server" Text="锁定" GroupName="radlock" />
                                <asp:RadioButton ID="radUnLock" runat="server" Text="未锁" GroupName="radlock" />
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
                            <div class="input"><span class="name">同行返佣：</span>
                                <asp:TextBox ID="txtProfessionCommissionStart" Style="width: 30px" runat="server"
                                    CssClass="text"></asp:TextBox>
                                % -
                                <asp:TextBox ID="txtProfessionCommissionEnd" Style="width: 30px" runat="server" CssClass="text"></asp:TextBox>
                                % </div>
                        </td>
                        <td>
                            <div class="input">
                                <span class="name">票证类型：</span>
                                <asp:RadioButton ID="rbnAllTicket" runat="server" Text="所有" Checked="true" GroupName="TicketType" />
                                <asp:RadioButton ID="rbnBspTicket" runat="server" Text="BSP" GroupName="TicketType" />
                                <asp:RadioButton ID="rbnB2bTicket" runat="server" Text="B2B" GroupName="TicketType" />
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
                            <div class="input"><span class="name">下级返佣：</span>
                                <asp:TextBox ID="txtSubordinateCommissionStart" Style="width: 30px" runat="server"
                                    CssClass="text"></asp:TextBox>
                                % -
                                <asp:TextBox ID="txtSubordinateCommissionEnd" Style="width: 30px" runat="server"
                                    CssClass="text"></asp:TextBox>
                                % </div>
                        </td>
                        <td> 
                            <div class="input">
                                <span class="name">政策状态：</span>
                                <asp:RadioButton ID="radAuditAll" runat="server" Text="所有" Checked="true" GroupName="radioone" />
                                <asp:RadioButton ID="radAudit" runat="server" Text="审核" GroupName="radioone" />
                                <asp:RadioButton ID="radUnAudit" runat="server" Text="未审" GroupName="radioone" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="btns">
                            <asp:Button ID="btnQuery" runat="server" CssClass="btn class1" Text="查 询" OnClick="btnQuery_Click" />
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
        <asp:GridView ID="grv_bargain" runat="server" CssClass="info" AutoGenerateColumns="false"
            Width="100%">
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
                <asp:TemplateField HeaderText="中转城市">
                    <ItemTemplate>
                        <span class="Transit">
                            <%#Eval("Transit")%>
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
                <asp:TemplateField HeaderText="票证行程">
                    <ItemTemplate>
                        <%#Eval("TicketType")%></ItemTemplate>
                </asp:TemplateField>
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
                <asp:TemplateField HeaderText="适用航班">
                    <ItemTemplate>
                        <span class="Include">
                            <%#Eval("Include")%></span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="提前天数" DataField="BeforehandDays" />
                <asp:BoundField HeaderText="发布价格" DataField="PriceInfo" />
                <asp:TemplateField HeaderText="航班日期">
                    <ItemTemplate>
                        <%#Eval("DepartureDates")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="适用舱位" DataField="Berths" />
                <asp:TemplateField HeaderText="返佣信息">
                    <ItemTemplate>
                        <%#Eval("Commission")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="供应方" DataField="Opearor" />
                <asp:BoundField HeaderText="产品审核" DataField="Sudit" />
                <asp:BoundField HeaderText="是否锁定" DataField="Lock" />
                <asp:BoundField HeaderText="是否挂起" DataField="Hang" />
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <a href='low_price_policy_info.aspx?id=<%#Eval("id") %>'>查看</a><br />
                        <%#Eval("LockTip")%>
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
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/checkboxchooice.js?20130416" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script> 
    <script src="/Scripts/PolicyModule/policylock.js?20130411" type="text/javascript"></script>
    <script src="/Scripts/selector.js?20121205" type="text/javascript"></script>
    <script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
    <%-- <script src="/Scripts/FixTable.js" type="text/javascript"></script>--%>
    <script src="/Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="/Scripts/OrderModule/QueryList.js?20130428" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $(".DepartureWeekFilter,.DepartureDateFilter,.Departure,.Arrival,.Transit,.Include,.Exclude").tipTip({ limitLength: 6, maxWidth: "300px" });
            $("#txtStartDate").focus(function () {
                WdatePicker({ skin: 'default', maxDate: '#F{$dp.$D(\'txtEndDate\')||\'2020-10-01\'}' });
            });
            $("#txtEndDate").focus(function () {
                WdatePicker({ skin: 'default', minDate: '#F{$dp.$D(\'txtStartDate\')}', maxDate: '2020-10-01' });
            });
            //            if ($("#grv_bargain tr").length > 0) {
            //                FixTable("grv_bargain", 17, 100);
            //            }
            SaveDefaultData();
        });
    </script>
</body>
</html>
