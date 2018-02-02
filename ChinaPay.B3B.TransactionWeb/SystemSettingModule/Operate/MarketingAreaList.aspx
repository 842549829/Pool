<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingAreaList.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.SystemSettingModule.Operate.MarketingAreaList" %>
<%@ Register Src="~/UserControl/Pager.ascx" TagName="Pager" TagPrefix="uc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <link rel="stylesheet" href="/Styles/public.css?20121118" />
       <link rel="stylesheet" href="/Styles/icon/fontello.css" />
<script src="/Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="hd">
     <h3 class="titleBg">销售区域设置</h3>
    </div>
    <div class="box-a">
    <div class="condition">
	    <table>
		    <colgroup>
			    <col class="w60"  />
			    <col class="w40" />
		    </colgroup>
		    <tr>
			    <td>
				    <div class="input">
					    <span class="name">销售区域：</span>
                        <asp:TextBox ID="txtAreaName" runat="server" CssClass="text"></asp:TextBox>
				    </div>
			    </td>
			    <td>
                  <asp:Button runat="server" ID="btnQuery" CssClass="btn class1" Text="查询" 
                        onclick="btnQuery_Click"/>
                         <asp:Button ID="btnAdd" runat="server" Text="新增" CssClass="btn class1" 
                    onclick="btnAdd_Click" />
              <asp:Button ID="btnDetele" runat="server" Text="删除" 
                    OnClientClick="return showCheckbox();" onclick="btnDetele_Click" CssClass="btn class2" />

               </td>
		    </tr>
	    </table>
    </div>
    </div>
    <div class="table">
      <asp:GridView ID="dataSource" runat="server" AutoGenerateColumns="False" 
            onrowcommand="dataSource_RowCommand">
              <Columns>
                <asp:TemplateField HeaderText="ID" Visible="false">
                  <ItemTemplate>
                    <asp:Label ID="lblId" runat="server" Text='<%#Eval("Id") %>'></asp:Label>
                  </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                  <HeaderTemplate>
                    <input id="chk_all" type="checkbox" onclick="checkAll(this)" />
                  </HeaderTemplate>
                  <ItemTemplate>
                    <asp:CheckBox ID="chkBox" runat="server" />
                  </ItemTemplate>
                </asp:TemplateField>
                  <asp:BoundField HeaderText="区域名称" DataField="Name" />
                  <asp:BoundField HeaderText="备注" DataField="Remark" />
                  <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                      <a href='AreaAddOrUpdate.aspx?Id=<%#Eval("Id") %>'>修改</a>
                      <asp:LinkButton ID="lnkDel" runat="server" CommandName="del" CommandArgument='<%#Eval("Id") %>' OnClientClick= 'return confirm("您确定要删除吗？")'>删除</asp:LinkButton>
                    </ItemTemplate>
                  </asp:TemplateField>
              </Columns>
          </asp:GridView>
              <div class="box" runat="server" visible="false" id="emptyDataInfo">
                 没有任何符合条件的查询结果
                 </div>
    </div>
<%--    <div>
        <div class="btns"><uc:Pager runat="server" id="pager" Visible="false"></uc:Pager></div>
    </div>--%>
    </form>
</body>
</html>
<script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
 <script type="text/javascript">
     $(function () {
         $("#btnQuery").click(function () {
             var areaNameObj = $("#txtAreaName");
             var areaName = $.trim(areaNameObj.val());
             if (areaName.length > 25) {
                 alert("销售区域格式错误！");
                 areaNameObj.select();
                 return false;
             }
         });
     })
     function checkAll(all) {
         var checks = document.getElementsByTagName("input");
         for (var i = 0; i < checks.length; i++) {
             if (checks[i].type == "checkbox") {
                 checks[i].checked = all.checked;
             }
         }
     }
     function showCheckbox() {
         var a = $(":checkbox:checked");
         if (a.length < 1) { alert("请先选择需要被删除的数据行！"); return false; }
         var flag = confirm('确认要删除吗？');
         if (flag == false) {
             return false;
         }
     }
       </script>

