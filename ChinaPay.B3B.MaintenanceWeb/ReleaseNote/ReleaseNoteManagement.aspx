<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReleaseNoteManagement.aspx.cs"
    Inherits="ChinaPay.B3B.MaintenanceWeb.ReleaseNote.ReleaseNoteManagement" %>

<%@ Register Src="../UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统更新日志管理</title>
    <link rel="stylesheet" type="text/css" href="/css/style.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>系统更新日志管理</span>&raquo;<span>更新日志管理</span>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png" />&nbsp;查询条件</dt>
                <dd class="searchbk">
                    <table width="97%" border="0" cellpadding="1" cellspacing="1" class="search">
                        <tr>
                            <th>
                                日期：
                            </th>
                            <td>
                                <asp:TextBox ID="txtStartTime" CssClass="input1" runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true})"></asp:TextBox>
                                -
                                <asp:TextBox ID="txtEndTime" CssClass="input1" runat="server" onfocus="WdatePicker({isShowClear:false,readOnly:true})"></asp:TextBox>
                            </td>
                            <th>
                                日志类型：
                            </th>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlReleaseType">
                                    <asp:ListItem Text="所有" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="B3B更新日志" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="国付通更新日志" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="fanwei">
                            <th>
                                发布范围：
                            </th>
                            <td colspan="3">
                                <asp:DropDownList runat="server" ID="ddlType">
                                    <asp:ListItem Text="所有" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="出票方可见" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="采购商可见" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="产品方可见" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="平台可见" Value="8"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr class="operator">
                            <td colspan="4">
                                <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="button button_a" OnClientClick="javascript:return QueryCheck();"
                                    OnClick="btnQuery_Click" />
                                <asp:Button ID="btnAdd" runat="server" Text="添加" CssClass="button button_a" OnClick="btnAdd_Click" />
                            </td>
                        </tr>
                    </table>
                </dd>
            </dl>
        </div>
        <div class="title">
            <dl>
                <dt>
                    <img src="../images/icon.png" />&nbsp;相应列表</dt>
                <dd>
                    <div class="gundong gundongy" id="data-list">
                        <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Center"
                            CssClass="tab3 list" OnRowCommand="gvRecords_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="更新日期" DataField="UpdateTime" DataFormatString="{0:yyyy-MM-dd}"
                                    ItemStyle-Width="80px" />
                                <asp:BoundField HeaderText="更新标题" DataField="Title" ItemStyle-Width="80px" />
                                <asp:TemplateField HeaderText="更新内容" ItemStyle-Width="400px" >
                                    <ItemTemplate>
                                        <div style="width: 400px; text-align: left;">
                                            <%#Eval("Context")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="发布范围" DataField="Type" ItemStyle-Width="80px" HtmlEncode="False" HtmlEncodeFormatString="False" />
                                <asp:BoundField HeaderText="操作人" DataField="Creator" ItemStyle-Width="80px" />
                                <asp:TemplateField HeaderText="操作" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <a href='ReleaseNote.aspx?id=<%#Eval("Id") %>&type=update'>编辑</a>
                                        <asp:LinkButton runat="server" CommandArgument='<%#Eval("Id") %>' CommandName='del'>删除</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="altrow" />
                            <HeaderStyle CssClass="searchListTitle" />
                        </asp:GridView>
                        <div id="showempty" runat="server" class="border_1" visible="false">
                            无满足条件的数据!</div>
                    </div>
                </dd>
            </dl>
            <div class="wpager">
                <div class="wpageright">
                    <uc1:Pager ID="pager" runat="server" Visible="false" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    <asp:HiddenField runat="server" ID="hidFanwei"/>
    </form>
</body>
</html>
<script src="/Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="/js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">
    function QueryCheck() {
        var startDate = document.getElementById('txtStartTime').value;
        var endDate = document.getElementById('txtEndTime').value;
        if (startDate > endDate) {
            alert("开始日期不能大于结束日期");
            return false;
        }
        return true;
    }
    if ($("#ddlReleaseType").val() == "1") {
        $(".fanwei").show();
        $("#hidFanwei").val("1");
    } else {
        $(".fanwei").hide();
        $("#hidFanwei").val("2");
    }
    $(function () {
        $("#ddlReleaseType").change(function () {
            if ($(this).val() == "1") {
                $(".fanwei").show();
                $("#hidFanwei").val("1");
            } else {
                $(".fanwei").hide();
                $("#hidFanwei").val("2");
            }
        });
        var containerWidth = $("#data-list").width();
        var horzionMaxWidth = Math.floor(containerWidth * 0.9);
        var horzionMinWidth = Math.floor(containerWidth * 0.1);
        $("#data-list").mouseover(function (e) {
            var widthh = e.originalEvent.x - $(this).offset().left || e.originalEvent.layerX - $(this).offset().left || 0;
            var tableWidth = $("#data-list table").width();
            var scrollWidht = parseFloat(tableWidth) - parseFloat(containerWidth);
            if (scrollWidht <= 0) {
                return;
            }
            if (parseFloat(widthh) > parseFloat(horzionMaxWidth)) {
                $(this).dequeue();
                $(this).animate({ scrollLeft: scrollWidht }, 3000);
            }
            if (parseFloat(widthh) < parseFloat(horzionMinWidth)) {
                $(this).dequeue();
                $(this).animate({ scrollLeft: '0' }, 3000);
            }
        }).mouseout(function () {
            $(this).stop();
        }).click(function () {
            $(this).stop();
        });
    })
</script>
