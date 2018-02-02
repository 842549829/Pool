<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResourceList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrderModule.Provide.ResourceList" %>

<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8">
    <title>提供座位</title>
</head><link rel="stylesheet" href="/Styles/public.css?20121118" />
<body>
    <form id="form1" runat="server" defaultbutton="btnQuery">
    <h3 class="titleBg">编码处理</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w33" />
                    <col class="w33" />
                    <col class="w33" />
                </colgroup>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">订单编号：</span><asp:TextBox runat="server" ID="txtOrderId" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">乘机人：</span><asp:TextBox runat="server" ID="txtPassenger" class="text textarea"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="input">
                            <span class="name">订单状态：</span><asp:DropDownList runat="server" ID="ddlStatus" AppendDataBoundItems="True">
                                                           </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="input">
                            <span class="name">创建日期：</span>
                            <asp:TextBox runat="server" ID="txtStartDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true})">
                            </asp:TextBox>
                            -
                            <asp:TextBox runat="server" ID="txtEndDate" class="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})">
                            </asp:TextBox><asp:HiddenField runat="server" ID="hdDefaultDate" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="btns" colspan="3">
                        <asp:Button ID="btnQuery" type="button" class="btn class1" runat="server" Text="查 询"
                            onclick="btnQuery_Click" />
                        <input type="button" value="清空条件" onclick="ResetSearchOption()" class="btn class2" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="table" id='data-list'>
        <asp:Repeater runat="server" ID="dataList" OnPreRender="AddEmptyTemplate" EnableViewState="False">
            <headertemplate><table>
              <colgroup>
                  <col class="w12"/>
                  <col class="w10"/>
                  <col class="w8"/>
                  <col class="w12"/>
                  <col class="w8"/>
                  <col class="w12"/>
                  <col class="w12"/>
                  <col class="w8"/>
                  <col class="w10"/>
              </colgroup>
		    <tr>
			    <th>订单号</th>
			    <th>航程</th>
			    <th>航班号</th>
			    <th>起飞时间</th>
			    <th>乘机人</th>
			    <th>票面价（元）<br />民航基金/燃油</th>
			    <th>创建时间</th>
			    <th>订单状态</th>
			    <th>锁定信息<br />
			        操作
			    </th>
		    </tr><tbody>
        </headertemplate>
        <itemtemplate>
		    <tr>
                <td><a href="OrderDetail.aspx?id=<%# Eval("OrderId") %>" class="obvious-a"><%# Eval("OrderId")%></a></td>
                <td><%# Eval("AirportPair") %></td>
                <td><%# Eval("FlightInfo") %></td>
                <td><%# Eval("TakeoffTime") %></td>
                <td><%# Eval("Passenger") %></td>
                <td><%# Eval("Fare") %><br /><%# Eval("AirportFee") %> / <%# Eval("BAF") %></td>
                <td><%# Eval("ProducedTime") %></td>
                <td class="b"><%# Eval("Status") %><span style='display:<%#(bool)Eval("RemindIsShow")?"":"none"%>'><a href='javascript:;' class='obvious urgent_btn urgent_order' content='<%#Eval("RemindContent") %>'>采购催单</a></span></td>
                <td>
                    <%#Eval("LockInfo")%>
                    <a href="javascript:Go('Supply.aspx','<%# Eval("OrderId") %>')" class="obvious-a b font-c">处理</a></td>
		    </tr>
        </itemtemplate>
            <footertemplate></tbody></table></footertemplate>
        </asp:Repeater>
    </div>
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    <div class="tips_box urgent_tips hidden urgent_orderDiv">
        <h2>催单内容</h2>
        <div class="tips_bd urgent_content">
            <p>催单内容</p>
        </div>
    </div>
    </form>
</body>
</html>
<script type="text/javascript" src="/Scripts/core/jquery.js"></script>
<script type="text/javascript" src="/Scripts/widget/common.js"></script>
<script src="/Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="/Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
<script src="/Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript">
    SaveDefaultData();
    $("#txtOrderId").OnlyNumber().LimitLength(13);
    function Go(url, id) {
        location.href = url + "?id=" + id + "&returnUrl=" + location.href;
    }
    $(function () {
        var urg = $(".urgent_orderDiv");
        $(".urgent_order").mouseenter(function () {
            $(".urgent_content p").html($(this).attr("content"));
            urg.removeClass("hidden");
            urg.css("left", $(this).offset().left - 100);
            urg.css("top", $(this).offset().top + 12);
        })
        urg.mouseleave(function () {
            urg.addClass("hidden");
        });
    })
</script>