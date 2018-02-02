<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Abolish.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.Abolish" %>
<%@ Import Namespace="ChinaPay.B3B.Service.SystemManagement.Domain" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>废票处理</title>
</head>
    <link rel="stylesheet" type="text/css" href="/Styles/masklayer/masklayer.css"/>
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style>
        #divPolicyRemarkContent{color:#000;}
    </style>
<body>
    <div runat="server" id="divError" class="column hd" visible="false"></div>
    <form id="form1" runat="server">
    <%--订单头部信息--%>
    <h3 class="titleBg">申请单详情</h3>
    <div class="form column">
        <table>
            <colgroup>
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
                <col class="w10" />
                <col class="w20" />
            </colgroup>
            <tr>
                <td class="title">申请单号</td>
                <td><asp:Label runat="server" ID="lblApplyformId" CssClass="obvious"></asp:Label></td>
                <td class="title">申请类型</td>
                <td><asp:Label runat="server" ID="lblApplyType" CssClass="obvious"></asp:Label></td>
                <td class="title">状态</td>
                <td><asp:Label runat="server" ID="lblStatus" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">订单编号</td>
                <td><a runat="server" id="linkOrderId" class="obvious-a"></a></td>
                <td class="title"> 产品类型</td>
                <td><asp:Label runat="server" ID="lblProductType" CssClass="obvious"></asp:Label></td>
                <td class="title">客票类型</td>
                <td><asp:Label runat="server" ID="lblTicketType" CssClass="obvious"></asp:Label></td>
            </tr>
            <tr>
                <td class="title">编码</td>
                <td><asp:Label runat="server" ID="lblPNR" CssClass="obvious"></asp:Label></td>
                <td class="title">采购商</td>
                <td><asp:Label runat="server" ID="lblRelation" CssClass="obvious"></asp:Label><a href="#" runat="server" id="hrefPurchaseName"></a></td>
                <td class="title">申请时间</td>
                <td><asp:Label runat="server" ID="lblAppliedTime"  CssClass="obvious"></asp:Label></td>
            </tr>
        </table>
    </div>
    <!--处理信息-->
    <div class="column">
        <h3 class="titleBg">处理信息</h3>
        <div class="table">
            <table>
                <tr>
                    <th>操作类型</th>
                    <th>提交时间</th>
                    <th>内容</th>
                    <th>操作人</th>
                </tr>
                <asp:Repeater runat="server" ID="ProcessInfo">
                    <itemtemplate>
                        <tr>
                            <td><%#Eval("Keyword") %></td>
                            <td><%#Eval("Time","{0:yyyy-MM-dd HH:mm}")%></td>
                            <td> <%#Eval("Content") %></td>
                            <td> <%#Eval("Account")%></td>
                        </tr>
                    </itemtemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <%--申请备注--%>
    <div runat="server" id="divPolicyRemark" visible="false">
        <h3 class="titleBg">政策备注</h3>
        <div runat="server" id="divPolicyRemarkContent" class="column table"></div>
    </div>
    <!--废票机票信息-->
    <div class="column table returnMoney">
        <h3 class="titleBg">废票机票信息：<a href='/SystemSettingModule/Role/AirlineRetreatChangeNew.aspx?Carrier=<%=Carrier %>'>查看该航空公司的废票规定</a></h3>
        <asp:Repeater runat="server" ID="FlightInfos">
            <HeaderTemplate>
                <table class="flightInfoTable">
                    <tbody>
                        <tr>
                            <th class="w15">起飞城市</th>
                            <th class="w15">到达城市</th>
                            <th class="w20">起飞时间</th>
                            <th class="w20">到达时间</th>
                            <th class="w10">航班号</th>
                            <th class="w10">舱位</th>
                            <th class="w10">折扣</th>
                        </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                   <td><%#Eval("Departure")%></td>
                   <td><%#Eval("Arrival")%></td>
                   <td><%#Eval("TakeoffTime")%></td>
                   <td><%#Eval("LandingTime")%></td>
                   <td><%#Eval("FightNo")%></td>
                   <td><%#Eval("Bunk")%></td>
                   <td><%#Eval("Discount")%></td>
                </tr>
            </ItemTemplate>
            <FooterTemplate></tbody></table>
            </FooterTemplate>
        </asp:Repeater>
        <br />
        <asp:Repeater ID="PassengerInfos" runat="server">
            <HeaderTemplate>
                <table class="passengerInfoTable">
                    <tbody>
                        <tr>
                            <th>类型</th>
                             <th>姓名</th>
                            <th>证件号</th>
                            <th>票号</th>
                            <th>票面价</th>
                            <th>机建/燃油</th>
                            <th style='display:<%=IsSpeical?"":"none" %>'>服务费</th>
                            <th>废票手续费</th>
                            <th>应退金额</th>
                            <th>实退金额</th>
                            <th style='display:<%=IsSpeical?"none":"" %>'><span class="obvious1">原卖出佣金</span></th>
                            <th><span class="obvious1">原卖出应收金额</span></th>
                        </tr>
                        
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                   <td><%#Eval("PassengerType")%></td>
                   <td><%#Eval("Name")%></td>
                   <td><%#Eval("Credentials")%></td>
                   <td><%#Eval("TicketNo")%></td>
                   <td><%#Eval("Fare")%></td>
                   <td><%#Eval("AirportFeeAndBAF")%></td>
                   <td style='display:<%=IsSpeical?"":"none" %>'><%#Eval("ServiceCharge")%></td>
                   <td><span class="b"><%#Eval("HandlingFee")%></span></td>
                   <td><span class="b price"><%#Eval("RefundAmount")%></span></td>
                   <td><span class="b price"><%#Eval("RealRefundAmount")%></span></td>
                   <td style='display:<%=IsSpeical?"none":"" %>'><span class="obvious1"><%#Eval("Commission")%></span></td>
                   <td><span class="obvious1"><%#Eval("Anticipation")%></span></td>
                   
                <tr />
            </ItemTemplate>
            <FooterTemplate>
                </tbody></table>
            </FooterTemplate>
        </asp:Repeater>
        <p class="invalidateInfoBox">
          <span>废票手续费合计：<b class="b price"><span id="RateAll" runat="server"></span> 元</b></span>
          <span>退款总金额：<b class="b price"><span id="RefundALL" runat="server"></span> 元</b></span>
        </p>
    </div>
    <!--多段废票提示-->
    <div class="importantBox broaden" id="MutilFlightTip" runat="server" visible="false">
        <p class="imTips">
            请注意，该废票为多段废票，请仔细核对该航程的票面价及民航基金，如果错误请通知平台修改；如因未认真核对而产生的废票纠纷由您自己进行处理，平台不承担任何责任！平台废票组电话：<asp:Label runat="server" ID="lblScrapPhone"></asp:Label>
        </p>
    </div>
    <!--分离出的新编码信息-->
    <div class="column table" style="display: <%=RequireSeparatePNR?"":"none;" %>">
        
            <h3 class="titleBg">分离出的新编码信息</h3>
        
        <div>
            <table>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">小编码：</span>
                            <asp:TextBox runat="server" CssClass="text" ID="txtPNR"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">大编码：</span>
                            <asp:TextBox runat="server" CssClass="text" ID="txtBPNR"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <!--操作按钮-->
    <div class="btns">
        <asp:Button class="btn class1" runat="server" id="btnAgree" Text="同意废票" 
            onclick="btnAgree_Click"/>
        <input type="button" class="btn class1" id="btnDeny" onclick="ShowReashoInput()" value="拒绝废票" />
        <asp:Button class="btn class1" runat="server" id="btnReleaseLockAndBack" 
            Text="返回并解锁" onclick="btnReleaseLockAndBack_Click"  />
        <button class="btn class2" runat="server" id="btnBack">返&nbsp;&nbsp;&nbsp;回</button>
    </div>
    <!--拒绝废票-->
    <div id="divDeny" class="form layers" style="display: none; width:500px; height:220px;">
        <h2>拒绝废票</h2>
        <table>
            <colgroup>
                <col class="w20" />
                <col class="w80" />
            </colgroup>
            <tr>
                <td class="title">类型</td>
                <td>
                    <div class="check">
                        <input type="radio" id="reasonType1" value='<%=((int)SystemDictionaryType.RefuseRefundSelfReason).ToString() %>' name="radioone" />
                        <label for="reasonType1">自身原因</label>
                        <input type="radio" id="reasonType2" value='<%=((int)SystemDictionaryType.RefuseRefundPlatformReason).ToString() %>'name="radioone" />
                        <label for="reasonType2">平台原因</label>
                        <input type="radio" id="reasonType3" value='<%=((int)SystemDictionaryType.RefuseRefundPurchaseReason).ToString() %>'name="radioone" />
                        <label for="reasonType3">采购原因</label>
                        <input type="radio" id="reasonType4" value='<%=((int)SystemDictionaryType.RefuseRefundOtherReason).ToString() %>'name="radioone" />
                        <label for="reasonType4">其他原因</label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="title">原因</td>
                <td>
                    <select id="selDenyReason"></select>
                </td>
            </tr>
            <tr>
                <td class="title">描述详情</td>
                <td><asp:textbox runat="server" CssClass="text" TextMode="MultiLine" ID="Reason" Width="340px" MaxLength="500" Height="60px" /></td>
            </tr>
            <tr class="btns">
                <td colspan="2">
                    <asp:Button ID="btnReset" runat="server" CssClass="class1 btn" Text="拒绝废票" OnClick="btnReset_Click" OnClientClick="return CheckReason()"/>
                    <input class="btn class2 close" type="button" id="btnCancelDeny" value="取&nbsp;&nbsp;&nbsp;消" onclick="CancleInput();" />
                </td>
            </tr>
        </table>
    </div>
    <div class="fixed"></div>
    <asp:HiddenField  runat="server" ID="hfdServicePhone"/>
    </form>
</body>
</html>
<script src="/Scripts/json2.js" type="text/javascript"></script>
<script src="/Scripts/widget/common.js?2013117" type="text/javascript"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/widget/form-ui.js" type="text/javascript"></script>
<script src="/Scripts/OrderModule/apply.js" type="text/javascript"></script>

<script src="/Scripts/OrderModule/Provide/Abolish.aspx.js" type="text/javascript"></script>
