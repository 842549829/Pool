<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemParameter.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.SystemManagement.SystemParameter" EnableSessionState="ReadOnly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>系统参数设置</title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs"> <span>当前位置:</span><span>系统管理</span>»<span>系统参数设置</span> </div>
        <div class="title">
            <dl>
                <dt><img src="../images/icon.png"/>&nbsp;相应列表</dt>
                <dd>
                <asp:GridView ID="gvSystemPerameter" runat="server" AutoGenerateColumns="False" EmptyDataText="无满足条件的数据!"
                        width="100%"  CssClass="tab3 list" >
                    <Columns>
                        <asp:TemplateField HeaderText="ID" Visible="False">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="参数类型" DataField="SystemParmType" ItemStyle-Width="15%"/>
                        <asp:BoundField HeaderText="参数值" DataField="Value" />
                        <asp:BoundField HeaderText="备注" DataField="Remark" ItemStyle-Width="50%"/>
                        <asp:TemplateField HeaderText="操作" HeaderStyle-Width="35px">
                            <ItemTemplate>
                               <a href='SystemParameter_Update.aspx?type=<%#Eval("TypeOf") %>&TypeName=<%#Eval("SystemParmType") %>'>修改</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="altrow" />
                    <HeaderStyle CssClass="searchListTitle" />
                </asp:GridView>
                </dd>
            </dl>
        </div>
    </div>
    <div class="clear"></div>
    </form>
</body>
</html>
<link rel="stylesheet" type="text/css" href="../css/style.css" />