<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="base_policy_import.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.PolicyModule.TransactionPolicy.base_policy_import" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>政策批量导入</title>
 </head>
   <link href="../../Styles/public.css?20121118" rel="stylesheet" type="text/css" />
    <link href="../../Styles/icon/fontello.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/skin.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/tipTip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .pager
        {
            margin: 0px 0px 20px 0px;
        }
        .pager table{float:left; width:300px;}
        .pager table tr td span,.pager table tr td a{float:left; width:10px;}
    </style>
<body>
    <form id="form1" runat="server">
    <div id="upload" runat="server">
        <h3 class="titleBg">
            政策批量导入：</h3>
        <br />
        <p>
            Excel表格模板下载： <a href="#" runat="server" id="lnkPolicyTemplate" class="obvious">普通政策模板</a>
            为保证政策数据的准确性，请先下载Excel表格模板，按模板内容编辑政策数据，并仔细检查政策是否有误.
        </p>
        <br />
        <p class="current">
            <asp:Label ID="lblWarnInfo" runat="server"></asp:Label></p>
        <br />
        <p>
            请上传已编辑好的政策EXCEL表格</p>
        <br />
        <p>
            文件路径：
            <asp:FileUpload ID="fudPath" runat="server" CssClass="text" />
            <asp:Button ID="btnImport" CssClass="btn class2" runat="server" Text="上传文件" OnClick="btnImport_Click" />
        </p>
        <br />
        <h3>
            政策批量导入注意事项：</h3>
        <br />
        <ul>
            <li>1.一次导入的数据不能超过500条。</li>
            <li>&nbsp;</li>
            <li>2.Excel中输入的字母不分大小写,返点数值必须在0-99.9之间,超出的请到录入页面录入 。</li>
            <li>&nbsp;</li>
            <li>3.下载模板的每个列名里都有备注，提示当前列怎么填写，鼠标放在上面可以查看到，其中的红色为必填，黑色可选填。</li>
            <li>&nbsp;</li>
            <li>4.导入后，可以在审核页面下批量审核，在已发布的普通政策页面下批量删除。</li>
            <li>&nbsp;</li>
        </ul>
    </div>
    <div id="confirm" runat="server" style="display: none">
        <h2>
            显示数据:</h2>
        <div class="table data-scrop">
            <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="true" AllowPaging="true"
                Width="2500px" OnPageIndexChanging="dataSource_PageIndexChanging" PageSize="10">
                <PagerStyle CssClass="pager" />
            </asp:GridView>
        </div>
        <asp:Button ID="btnConfirm" CssClass="btn class1" Text="审核" runat="server" OnClick="btnConfirm_Click" />
    </div>
    <div id="reCheck" runat="server" style="display: none">
        <h2>
            复核数据:</h2>
        <div>
            <b>本次共导入<asp:Label ID="lblTotal" runat="server" Text="" ForeColor="Red"></asp:Label>条政策，
                可用政策条数为<asp:Label ID="lblAvailable" runat="server" Text="" ForeColor="Red"></asp:Label>条，
                格式错误条数为<asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>条
                <a href="javascript:;" id="a_error">
                    <asp:Label ID="lblerrordetails" runat="server" ForeColor="Red">查看错误详情</asp:Label></a><br />
            </b>
        </div>
        <div class="table data-scrop">
            <asp:GridView ID="checkDataSource" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                Width="2500px" OnPageIndexChanging="checkDataSource_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="行程类型">
                        <ItemTemplate>
                            <%#Eval("VoyageType")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="航空公司" DataField="Airline" />
                    <asp:BoundField HeaderText="OFFICE号" DataField="OfficeCode" />
                    <asp:BoundField HeaderText="自定义编号" DataField="CustomerCode" />
                    <asp:TemplateField HeaderText="出发城市">
                        <ItemTemplate>
                            <%#Eval("Departure")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="Departure" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="到达城市">
                        <ItemTemplate>
                            <%#Eval("Arrival")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="Arrival" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="排除航段">
                        <ItemTemplate>
                            <%#Eval("ExceptAirways")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="ExceptAirways" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="去程不适用航班号">
                        <ItemTemplate>
                            <%#Eval("DepartureExclude")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="DepartureExclude" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="去程适用航班号">
                        <ItemTemplate>
                            <%#Eval("DepartureInclude")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="DepartureInclude" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="回程不适用航班号">
                        <ItemTemplate>
                            <%#Eval("ReturnExclude")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="ReturnExclude" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="回程适用航班号">
                        <ItemTemplate>
                            <%#Eval("ReturnInclude")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="ReturnInclude" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="出票条件">
                        <ItemTemplate>
                            <%#Eval("DrawerCondition") %>
                        </ItemTemplate>
                        <ItemStyle CssClass="DrawerCondition" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="政策备注">
                        <ItemTemplate>
                            <%#Eval("Remark")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="Remark" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="去程日期">
                        <ItemTemplate>
                            <%#Eval("DepartureDates")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="出票日期" DataField="ETDZDate" />
                    <asp:TemplateField HeaderText="排除日期">
                        <ItemTemplate>
                            <%#Eval("DepartureDatesFilter")%></ItemTemplate>
                        <ItemStyle CssClass="DepartureDatesFilter" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="适用班期">
                        <ItemTemplate>
                            <%#Eval("DepartureWeekFilter")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="DepartureWeekFilter" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="舱位">
                        <ItemTemplate>
                            <%#Eval("Berths")%>
                        </ItemTemplate>
                        <ItemStyle CssClass="Berths" />
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="政策类型" DataField="TicketType" />
                    
                    <asp:TemplateField HeaderText="政策返佣">
                        <ItemTemplate>
                            <%#Eval("Commission")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="是否适用往返降舱" DataField="SuitReduce" />
                    <asp:BoundField HeaderText="审核状态" DataField="Sudit" />
                    <asp:BoundField HeaderText="是否换编码出票" DataField="ChangePNR" />
                    <asp:BoundField HeaderText="起飞前2小时内可用B2B出票" DataField="PrintBeforeTwoHours" />
                </Columns>
                <PagerStyle CssClass="pager" />
            </asp:GridView>
        </div>
        <asp:Button ID="btnCheck" runat="server" CssClass="btn class1" Text="提交" OnClick="btnCheck_Click" />
    </div>
    <div id="errorList" runat="server" style="display: none">
        <h2>
            查看错误详情</h2>
        <div class="table data-scrop" runat="server">
            <asp:GridView ID="errorDataSource" runat="server" AllowPaging="true" Width="2500px"
                OnPageIndexChanging="errorDataSource_PageIndexChanging">
                <PagerStyle CssClass="pager" />
            </asp:GridView>
        </div>
        <input type="button" value="返回" class="btn class2" id="btnReturn" />
    </div>
    </form>
</body>
</html>
<script src="../../Scripts/core/jquery.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.tipTip.minified.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () {
        $(".Departure,.Arrival,.Berths").tipTip({ limitLength: 3, maxWidth: "330px" });
        $(".DepartureExclude,.DepartureInclude,.DepartureWeekFilter,.DepartureDatesFilter,.ReturnExclude,.ReturnInclude,.ReturnDatesFilter,.ExceptAirways,.Remark,.DrawerCondition").tipTip({ limitLength: 10 });
        $("#a_error").click(function () {
            $("#reCheck").css("display", "none");
            $("#errorList").css("display", "");
        });
        $("#btnReturn").click(function () {
            $("#reCheck").css("display", "");
            $("#errorList").css("display", "none");
        });
    })
</script>
