<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseDefault.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.PurchaseDefault" %>

<%@ Register TagPrefix="uc" TagName="Footer_1" Src="~/UserControl/Footer.ascx" %>
<%@ Register TagPrefix="uc" TagName="Header" Src="~/UserControl/Header.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
 </head>
   <link href="/Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="/Styles/skin.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/flightQuery.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .gobackdate
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        if (window.top != window.self) {
            window.top.location = window.location;
        }
    </script>
<body>
    <form id="form1" runat="server">
    <uc:Header runat="server" ID="ucHeader" FlightText="我的机票"></uc:Header>
    <div id="bd">
        <div class="content clearfix">
            <p id="userinfor" runat="server">
            </p>
            <p id="userscore">
                可用积分：<asp:Literal Text="" ID="txtIntegral" runat="server" />分 <a href="/Index.aspx?redirectUrl=/IntegralCommodity/CommodityShowList.aspx">
                    积分商城</a>&nbsp;| <a href="/Index.aspx?redirectUrl=/IntegralCommodity/IntegralZengZhang.aspx">
                        增长记录</a>
            </p>
        </div>
        <a href="#" runat="server" id="newRelease" visible="false">
            <div class="releasetips">
                <div class="releasecon">
                    <div class="releasenew">
                    </div>
                </div>
            </div>
        </a>
        <div id="hotCity" class="tipControl" style="display: none;">
            <span>热门城市(支持汉字/拼音/三字码)</span><img alt="关闭" src="/Images/ico_close.gif" width="11"
                height="11" />
            <div class="hotCityNav">
                <span class="active">热门</span><span>A-G</span><span>H-L</span><span>M-T</span><span>W-Z</span></div>
            <ul class="cityList">
            </ul>
            <ul class="cityList">
            </ul>
            <ul class="cityList">
            </ul>
            <ul class="cityList">
            </ul>
            <ul class="cityList">
            </ul>
        </div>
        <div id="divAirlineSelector" class="airlineSelector tipContro l" style="display: none;">
            <span style="color: #F60">请选择航空公司</span><img alt="关闭" src="/Images/ico_close.gif"
                width="11" height="11" />
            <ul class="clearfix">
            </ul>
        </div>
        <div class="content clearfix" id="searchFlight">
            <div class="inqueryBox leftBox">
                <div class="boxTitle">
                    <h4 id="plane">
                        航班查询/预订
                    </h4>
                </div>
                <table>
                    <tr>
                        <td class="title">
                            航班类型：
                        </td>
                        <td>
                            <input type="radio" id="radOneWay" name="vayageType" value="0" checked="checked" /><label
                                for="radOneWay">单程</label>
                            <input type="radio" id="radRoundWay" name="vayageType" value="1" /><label for="radRoundWay">往返</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            出发城市：
                        </td>
                        <td>
                            <div class="borderBox">
                                <input type="hidden" id="txtDepartureValue" runat="server" />
                                <input type="text" id="txtDeparture" class="inputBox" autocomplete="false" onblur="airportControlBlured($('#txtDepartureValue'),$('#hotCity'),$(this));"
                                    runat="server" onkeyup="showCitis($('#txtDepartureValue'),$('#hotCity'),$(this),event);"
                                    onfocus="showCitis($('#txtDepartureValue'),$('#hotCity'),$(this),event);$(this).select();" />
                                <span onclick="showCitiesSelectWindow($('#txtDepartureValue'),$('#hotCity'),$('#txtDeparture'));"
                                    class="cityBox" title="选择出发城市"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            到达城市：
                        </td>
                        <td>
                            <div class="borderBox">
                                <input type="hidden" id="txtArrivalValue" runat="server" />
                                <input type="text" id="txtArrival" class="inputBox" autocomplete="false" onblur="airportControlBlured($('#txtArrivalValue'),$('#hotCity'),$(this));"
                                    runat="server" onkeyup="showCitis($('#txtArrivalValue'),$('#hotCity'),$(this),event);"
                                    onfocus="showCitis($('#txtArrivalValue'),$('#hotCity'),$(this),event);$(this).select();" />
                                <span onclick="showCitiesSelectWindow($('#txtArrivalValue'),$('#hotCity'),$('#txtArrival'));"
                                    class="cityBox">
                                    <input type="hidden" id="txtAirlineValue" />
                                </span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            出发日期：
                        </td>
                        <td>
                            <div class="borderBox">
                                <input type="text" id="txtGoDate" runat="server" class="inputBox" onfocus="WdatePicker({isShowClear:false,readOnly:true,minDate:'%y-%M-%d',doubleCalendar:true,startDate:'%y-%M-{%d+1}'});" />
                                <span onclick="WdatePicker({isShowClear:false,el:'txtGoDate',readOnly:true,minDate:'%y-%M-%d',doubleCalendar:true,startDate:'%y-%M-{%d+1}'})"
                                    class="arrivalBox"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="title">
                            <span class="gobackdate">返程日期：</span>
                        </td>
                        <td>
                            <div class="borderBox gobackdate">
                                <input type="text" id="txtBackDate" class="inputBox" onfocus="WdatePicker({isShowClear:false,el:'txtBackDate',readOnly:true,minDate:'#F{$dp.$D(\'txtGoDate\')}',onpicked:triggerRounadWay,doubleCalendar:true})" />
                                <span onclick="WdatePicker({isShowClear:false,el:'txtBackDate',readOnly:true,minDate:'#F{$dp.$D(\'txtGoDate\')}',onpicked:triggerRounadWay,doubleCalendar:true})"
                                    class="returnBox"></span>
                            </div>
                        </td>
                    </tr>
                </table>
                <input type="button" id="btnQueryFlight" value="查询航班" class="inquireBtn btnFixed" />
                <button type="button" id="changeCity" onclick="change()">
                    &nbsp;</button>
            </div>
            <div class="inqueryBox">
                <div class="boxTitle">
                    <h4 id="PNR">
                        PNR创建订单
                    </h4>
                </div>
                <table>
                    <tr>
                        <td style="width: 113px;">
                            <asp:RadioButton runat="server" ID="radAdultPNR" Checked="True" Text="成人编码" GroupName="radPNRType" />
                        </td>
                        <td>
                            <asp:RadioButton runat="server" ID="radChildrenPNR" Text="儿童编码" GroupName="radPNRType" />
                        </td>
                    </tr>
                    <tr class="childrenPNR">
                        <td>
                            儿童PNR编码：
                        </td>
                        <td>
                            <input type="text" id="txtChildrenPNRCode" class="PNRText" runat="server" />
                        </td>
                    </tr>
                    <tr class="AdultPNRCode">
                        <td>
                            备注成人编码：
                        </td>
                        <td>
                            <input type="text" id="txtAdultPNRCode" class="PNRText" runat="server" />
                        </td>
                    </tr>
                    <tr id="pnrCode">
                        <td>
                            PNR编码：
                        </td>
                        <td>
                            <asp:TextBox CssClass="PNRText" runat="server" ID="txtPNRCode" />
                        </td>
                    </tr>
                    <tr class="emptyPNR">
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button Text="提交PNR编码" ID="btnPNRImport" runat="server" CssClass="inquireBtn"
                                OnClientClick="return CheckPNR()" OnClick="btnPNRCodeImport_Click" />
                        </td>
                    </tr>
                </table>
                <p class="PNRprompt">
                    编码导入建议行程：单程（普通、特价）</p>
                <div class="PNRTips1">
                    <a href="/Index.aspx?redirectUrl=/FlightReserveModule/PNRImport.aspx" id="PAT"><span
                        class="arrowR"></span>更多行程低打<span style="color:#FF1A1A;">请点此处:</span><br />
                        散客 | 团队（往返、中转、多段联程、缺口程）</a>
                </div>
            </div>
            <div class="inqueryBox rightBox">
                <div class="boxTitle1">
                    <h4 class="header">
                        最新公告
                    </h4>
                    <a href="/Index.aspx?redirectUrl=/SystemSettingModule/Role/AnnounceList.aspx">更多&gt;&gt;</a>
                </div>
                <ul class="announcement clearfix">
                    <asp:Repeater runat="server" ID="dataList">
                        <ItemTemplate>
                            <li title='<%#Eval("Title") %>'><a href="/Index.aspx?redirectUrl=/SystemSettingModule/Role/AnnounceInfo.aspx?Id=<%#Eval("Id") %>">
                                <%#(Eval("Title").ToString().Length>15?(Eval("Title").ToString().Substring(0,15)+"..."):Eval("Title")) %></a>
                                <span>
                                    <%#Eval("PUblishTime","{0:yyyy-MM-dd}") %></span> </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <div class="content clearfix">
            <div class="recommend" id="recommend">
                <div class="recommendTitle">
                    <h4 class="header">
                        特价推荐
                    </h4>
                    <ul id="CityList">
                    </ul>
                </div>
                <div id="errorInfo" style="display: none;">
                    加载特价信息失败……</div>
            </div>
            <div class="help rightBox">
                <div class="boxTitle1">
                    <h4 class="header">
                        采购小助手
                    </h4>
                </div>
                <ul class="clearfix">
                    <li><a href="/Index.aspx?redirectUrl=/OrderModule/Purchase/OrderList.aspx">
                        <div class="assistantPic" id="myTicket">
                            &nbsp;</div>
                        我的机票</a></li>
                    <li><a href="/Index.aspx?redirectUrl=/IntegralCommodity/IntegralXiaoFei.aspx">
                        <div class="assistantPic" id="myPoints">
                            &nbsp;</div>
                        我的积分</a></li>
                    <li><a href="/Index.aspx?redirectUrl=/OrganizationModule/RoleModule/CompanyInfoMaintain/CompanyInfoMaintain.aspx">
                        <div class="assistantPic" id="information">
                            &nbsp;</div>
                        我的信息</a></li>
                    <li><a href="/Index.aspx?redirectUrl=/OrganizationModule/RoleModule/UpdatePassword.aspx">
                        <div class="assistantPic" id="modifyPwd">
                            &nbsp;</div>
                        修改密码</a></li>
                    <li><a href="/Index.aspx?redirectUrl=/OrganizationModule/Account/AccountInformation.aspx">
                        <div class="assistantPic" id="account">
                            &nbsp;</div>
                        资金账号</a></li>
                    <li><a href="/About/help.aspx">
                        <div class="assistantPic" id="helpCenter">
                            &nbsp;</div>
                        帮助中心</a></li>
                </ul>
                <img src="/images/line.png" class="clearfix" />
                <p id="hotline">
                    客服热线：<asp:Literal Text="" ID="ServiceTelephone" runat="server" /></p>
            </div>
        </div>
    </div>
    <uc:Footer_1 runat="server" ID="ucFooter"></uc:Footer_1>
    <!-- PAT内容输入区  -->
    <a id="popWinTrigger" style="display: none" data="{type:'pop',id:'div_PATContent'}">
    </a>
    <div class="layer4 importLayer" id="div_PATContent" style="display: none">
        <h4>
            操作提示：<a href="javascript:void(0);" class="close">关闭</a></h4>
        <p class="important">
            请输入PAT信息</p>
        <div class="addBox">
            <p>
                请在此处粘贴PAT内容</p>
            <textarea rows="2" cols="20" id="txtPATContent" runat="server"></textarea>
            <div class="layerBtns">
                <input type="button" id="btnPARSubmit" value="提交" class="btn class1" onclick="CheckPAT()" />
                <a href="#" class="btn class2 close">取消</a>
            </div>
        </div>
        <div class="tipsBox">
            <p>
                操作帮助：在RT编码后，“成人编码”请您再执行“PAT:A”指令，如：</p>
            <p>
                &gt;<b class="b">PAT:A</b></p>
            <p>
                PAT:A</p>
            <p>
                01 M FARE:CNY1520.00 TAX:CNY50.00 YQ:CNY130.00 TOTAL:1700.00</p>
            <p>
                &gt;>SFC:01
            </p>
            <p>
                &gt;</p>
            <p>
                操作帮助：在RT编码后，“儿童编码”请您在执行“PAT:A*CH”指令，如：</p>
            <p>
                &gt;<b class="b">PAT:A*CH</b></p>
            <p>
                PAT:A</p>
            <p>
                01 M FARE:CNY1520.00 TAX:CNY50.00 YQ:CNY130.00 TOTAL:1700.00
            </p>
            <p>
                &gt;SFC:01</p>
            <p>
                &gt;</p>
            <p>
                最后将PAT指令所获得的内容复制到PAT内容输入框中，点击确认生成订单</p>
        </div>
    </div>
    </form>
    <%--紧急公告 --%>
    <div id="Announce">
        <a id="divOpcial" style="display: none;" data="{type:'pop',id:'div_Announce'}"></a>
        <div id="div_Announce" class="form layer layer2" style="display: none; width: 800px;">
            <h4>
                紧急公告 <a href="javascript:;" class="userClose">关闭 </a>
            </h4>
            <div class="layer2TitleBox" style="width: 800px;">
                <h3>
                    <span id="lblTitle"></span>
                </h3>
                <span>发布时间：<i id="lblPublishTime">2012-09-02</i></span>
            </div>
            <div class="layer2ContentBox" id="content" style="clear: both; width: 800px;">
                <p>
                </p>
            </div>
            <div id="pager" class="obvious-a fr" style="padding: 20px;">
                <a href="javascript:;" class="obvious-a " id="prvIndex">上一条</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a
                    href="javascript:;" id="nextIndex" class="obvious-a ">下一条</a></div>
            <div>
                <div style="text-align: center; width: 800px;" class="btns clearfix">
                    <input type="button" value="关闭" class="btn class2 userClose" id="userClose" title="关闭" /></div>
            </div>
        </div>
    </div>
    <a id="divWarnInfo" style="display: none" data="{type:'pop',id:'div_WarnInfo'}">
    </a>
    <div class="layer4 importLayer" id="div_WarnInfo" style="display: none">
        <h4>
            操作提示：<a href="javascript:void(0)" class="close">关闭</a></h4>
        <p class="important">
            <asp:Label ID="lblWarnInfo" runat="server" Text="请输入正确的PNR内容"></asp:Label>
        </p>
        <div class="layerBtns">
            <a href="javascript:void(0);" class="btn class1 close">确定</a>
        </div>
    </div>
    <script type="text/x-jquery-teml" id="flightInfoTmpl"><li><span class="flightInfo"><a href="javascript:SearchFlight('${DepartureCode}','${ArrivalCode}','${Date}')">${Departure}-${Arrival}</a> <span>${Date}</span>   <span class="price">￥${Fare}</span></span></li>
    </script>
    <script type="text/x-jquery-tmpl" id="CityListTmpl"><li><a onmouseover="Load(this,'${Code}')" href="javascript:Load(this,'${Code}')">${Name}</a></li></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Scripts/Global.js?20121118"></script>
    <script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
    <script src="/Scripts/widget/template.js" type="text/javascript"></script>
    <script src="/Scripts/widget/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        needLoadCookie = false;
        function ReSet() {
            $(".jfocus").css("width", "100%");
        };
        var hideSuggest = true;
        $(function () {
            $("#radAdultPNR").click(function () {
                $(".childrenPNR,.AdultPNRCode").hide();
                $("#pnrCode,.emptyPNR").show();
            });
            $("#radChildrenPNR").click(function () {
                $(".childrenPNR,.AdultPNRCode").show();
                $("#pnrCode,.emptyPNR").hide();

            });
            sendPostRequest("/FlightHandlers/Recommend.ashx/QueryRecommendCities", "", function (citys) {
                $("#CityListTmpl").tmpl(citys).first().find("a").addClass("selectedCity").end().end().appendTo("#CityList");
                location.href = $("#CityList a").first().attr("href");
            }, function () { $("#CityList").html("加载数据出错"); });

            if ($("#radChildrenPNR").is(":checked")) $("#radChildrenPNR").trigger("click");

            $("#txtDeparture,#txtArrival,#txtGoDate,#txtBackDate,#from1").keypress(function (e) {
                if (e.keyCode == 13) {
                    return false;
                }
            });
        });
        function Load(sender, code) {
            var container = $("#recommend");
            if ($(sender).is("a")) {
                $(".selectedCity", container).removeClass("selectedCity");
                $(sender).addClass("selectedCity");
            }
            var currentList = $("." + code, container);
            if (currentList.size() > 0) {
                $("ul", container).not("#CityList").hide();
                currentList.show();
            } else {
                sendPostRequest("/FlightHandlers/Recommend.ashx/QueryRecommendInfos", JSON.stringify({ "code": code }),
                    function (rsp) {
                        $("ul", container).not("#CityList").hide();
                        $("#flightInfoTmpl").tmpl(rsp).appendTo("#recommend").wrapAll("<ul class='clearfix recommendList " + code + "'></ul>");
                    }, function () { $("#errorInfo").show(); });
            }
        }
        function CheckPNR() {
            var pnr;
            var pnrReg = /^[\w\d]{6}$/;
            if ($("#radAdultPNR").is(":checked")) {
                pnr = $.trim($("#txtPNRCode").val());
                if (pnr == "") {
                    $("#divWarnInfo").click();
                    $("#lblWarnInfo").text("请先输入PNR");
                    return false;
                }
                if (!pnrReg.test(pnr)) {
                    $("#divWarnInfo").click();
                    $("#lblWarnInfo").text("请输入正确的编码");
                    return false;
                }
            } else {
                pnr = $.trim($("#txtAdultPNRCode").val());
                if (pnr == "") {
                    $("#divWarnInfo").click();
                    $("#lblWarnInfo").text("请先输入成人PNR");
                    return false;
                }
                if (!pnrReg.test(pnr)) {
                    $("#divWarnInfo").click();
                    $("#lblWarnInfo").text("请输入正确的成人编码");
                    return false;
                }
                pnr = $.trim($("#txtChildrenPNRCode").val());
                if (pnr == "") {
                    $("#divWarnInfo").click();
                    $("#lblWarnInfo").text("请先输入儿童PNR");
                    return false;
                }
                if (!pnrReg.test(pnr)) {
                    $("#divWarnInfo").click();
                    $("#lblWarnInfo").text("请输入正确的儿童编码");
                    return false;
                }
            }
            //return NeedAddPAt();
            return true;  //提交时不需要立即输入PAT
        }

        function triggerRounadWay() {
            $("#radRoundWay").trigger("click");
            return false;
        }

        function SearchFlight(departure, arrival, date) {//转到航班查询页面查询航班
            var now = new Date();
            var searchUrl = "/FlightReserveModule/FlightQueryResult.aspx?source=1&departure={0}&arrival={1}&goDate={2}-{3}";
            var year = now.getFullYear();
            var selectedDateNum = /^(\d{2})-(\d{2})$/.exec(date);
            if (parseInt(selectedDateNum[1], 10) < now.getMonth() + 1
                || parseInt(selectedDateNum) == now.getMonth() + 1 && parseInt(selectedDateNum[2], 10) < now.getDate()) {
                year++;
            }
            location.href = searchUrl.format(departure, arrival, year, date);
        }

        var hasPat = false; var patReg = /<%=PatReg %>/i;
        function NeedAddPAt() {
            if (hasPat) return true;
            $("#popWinTrigger").click();
            return false;
        }

        function CheckPAT() {
            var contentCtl = $("#txtPATContent");
            if ($.trim(contentCtl.val()) == "") {
                alert("请输入PAT内容");
                return false;
            }
            if (!patReg.test(contentCtl.val())) {
                alert("PAT内容格式不正确");
                return false;
            }
            hasPat = true;
            $("#btnPNRImport").trigger("click");
        }
    </script>
    <script src="/Scripts/FlightModule/queryControl.js" type="text/javascript"></script>
</body>
</html>