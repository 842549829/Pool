<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProvideETDZSpeedStatisticReport.aspx.cs"
    Inherits="ChinaPay.B3B.TransactionWeb.ReportModule.ProvideETDZSpeedStatisticReport" %>
<%@ Register TagPrefix="uc" TagName="Pager" Src="~/UserControl/Pager.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>当日票量分析报表</title>
</head>
    	<style type="text/css">
		.tatistics-l{
			border-right: 2px solid #dedede;
			margin-right: 5%;
			min-width: 140px;
			width: 20%;
		}
		.tatistics-r{
			width: 74%;
		}
		.tatistics-l p{
			height: 35px;
			padding: 0 10px 10px;
		}
	</style>

    <script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
<body>
    <form id="query" runat="server">
    <h3 class="titleBg">查看出票效率</h3>
		<div class="box-a clearfix">
			<div class="fl tatistics-l">
				<p>
                    <asp:CheckBox Text="按航空公司分组" Checked="true" runat="server" ID="ByCarriar" CSSClass=".adio_reset" />
				</p>
				<p><asp:CheckBox Text="按客票类型分组" Checked="true" runat="server" ID="ByTicketType" CSSClass=".adio_reset" />
				</p>

			</div>
			<table class="fl tatistics-r condition">
				<tbody>
				    <tr>
					<td>
						<div class="input">
							<span class="name">起止时间：</span>
                            <asp:TextBox runat="server" ID="txtStatFrom" CssClass="text text-s" onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"/>   至 
                            <asp:TextBox runat="server"  ID="txtStatTo" CssClass="text text-s"  onfocus="WdatePicker({isShowClear:false,readOnly:true,maxDate:'%y-%M-%d'})"/>
						    <asp:HiddenField runat="server" id="hdDefaultDate" />
						</div>
					</td>
                    <td>
                        &nbsp;
                    </td>
				</tr>
				<tr>
					<td>
						<div class="input">
							<span class="name">客票类型：</span>
							<asp:DropDownList ID="ddlTicketType" runat="server" Width="60px" CssClass="selectarea" AppendDataBoundItems="True">
                                <asp:ListItem Text="全部" Value=""></asp:ListItem>
                            </asp:DropDownList>
						</div>
					</td>
					<td>
                        <asp:Button Text="查 询" ID="btnQuery" runat="server" CssClass="btn class1" OnClick="btnQuery_Click" /> 
                        <input type="button" class="btn class2" id="clearCondition" value="清空条件" onclick="ResetSearchOption('.box-a,.condition')" />
					</td>
				</tr>
				<tr>
					<td>
						<div class="input">
							<span class="name">航空公司：</span>
							<asp:DropDownList ID="ddlAirlines" runat="server" CssClass="selectarea">
                            </asp:DropDownList>
						</div>
					</td>
					<td>
						&nbsp;
					</td>
				</tr>
			</tbody>
            </table>
		</div> 
           <div class="table data-scrop" id='data-list'>
        <asp:GridView runat="server" ID="dataList"  EnableViewState="false" ></asp:GridView>
        <div class="box" runat="server" visible="false" id="emptyDataInfo">
            没有符合条件的查询结果
        </div>
    </div>
    <br />
    <div class="btns">
        <uc:Pager runat="server" ID="pager" Visible="false"></uc:Pager>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">
    pageName = "ETDZSpeed";
    $(function ()
    {
        SaveDefaultData();
    });
</script>
<script src="../Scripts/core/jquery.js" type="text/javascript"></script>
<script src="../Scripts/selector.js?20121205" type="text/javascript"></script>
<script src="../Scripts/DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="../Scripts/Report/common.js" type="text/javascript"></script>
<script src="../Scripts/Global.js?20121118" type="text/javascript"></script>
<script src="../Scripts/OrderModule/QueryList.js?20121119" type="text/javascript"></script>
