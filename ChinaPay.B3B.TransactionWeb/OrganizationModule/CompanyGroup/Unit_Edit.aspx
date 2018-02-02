
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Unit_Edit.aspx.cs" Inherits="ChinaPay.B3B.TransactionWeb.OrganizationModule.CompanyGroup.Unit_Edit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<meta http-equiv="Pragma" content="no-cache">
<meta http-equiv="Cache-Control" content="no-cache">
<meta http-equiv="Expires" content="0">

<head runat="server">
    <title>添加修改公司组</title>
    <script src="../../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
</head>
    <style type="text/css">
        .op_item .btns input
        {
            display: block;
            float: none;
            margin: 10px 10px 0 0;
        }
        .hidden
        {
            display: none;
        }
        .form table td
        {
            padding:3px;
        }
        .airLine 
        {
            width:290px;
        }
        .IE7\.0 .Item .btn
        {
            padding-left:0;
            padding-right:0;
        }
    </style>
    <script type="text/javascript">
        var isAdd = <%=IsAdd?"true":"false" %>;
    </script>
<body>
    <form id="form1" runat="server">
    <h3 class="titleBg">
        添加/修改公司组</h3>
    <div class="box-a">
        <div class="condition">
            <table>
                <colgroup>
                    <col class="w40" />
                    <col class="w30" />
                    <col class="w30" />
                </colgroup>
                <tbody>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">组别名称：</span>
                                <asp:TextBox ID="txtGroupName" runat="server" CssClass="text" MaxLength="25">
                                </asp:TextBox>
                                <span class="obvious" id="lblGroupName"></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input">
                                <span class="name">组别描述：</span>
                                <asp:TextBox TextMode="MultiLine" runat="server" CssClass="text" id="describe" style="width: 400px; height: 60px;" MaxLength="200"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <h2>
        不允许采购其他代理发布的政策设置</h2> 
    <div class="hidden">
        <div class="form" id="template" >
        <div class="fr">
          <span style="padding-right:5px">
             <input id="btnAddItem" class="class1 btn" type="button" value="新增" onclick="addForm()" /></span>
             <input id="btnDel" class="btn class2 deleteButton" type="button" value="删除" />
            </div>
            <table style="border-right:1px solid #CECECE">
                <tr>
                    <td class="title">
                        受限航空公司
                    </td>
                    <td>
                        <input type="radio" class="checkAll" name="checkAll"/><label for="checkAll">全选</label>
                        <input type="radio" class="checkOpposite" name="checkAll"/><label for="checkAll">反选</label>
                        <br/>
                        <br/>
                        <table class="airLine">
                        </table>
                    </td>
                    <td class="title">
                        受限出港城市
                    </td>
                    <td>
                        <input class="txtAirports text" style="width: 300px;" type="text" />
                        <span>多个城市，请用”/”分隔开，所有城市请用”*”代替
</span>
                        <br />
                        <div class="op_item">
                            <select multiple="multiple" class="op_con op_con_l source">
                            </select>
                            <%--<select multiple="multiple" class="op_con op_con_r target">
                            </select>--%>
                            <div class="btns">
                                <input type="button" class="btn class2 addAll" value="全部添加" />
                                <input type="button" class="btn class2 add" value="添　　加" />
                                <input type="button" class="btn class2 remove" value="删　　除" />
                                <input type="button" type="button" class="btn class2 removeAll" value="全部删除" />
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divInfo" class="hidden">
        
    </div>
    <div class="btns">
        <input id="btnSave" class="class1 btn" type="button" value="保存" onclick="return btnSave_onclick()" />
        
        <input type="button" class="btn class2" onclick="location.href='CompanyGroupList.aspx'" value="返回" />
    </div>
    <input type="hidden" id="hidId" runat="server" />
    <input type="hidden" id="hidCompanyGroupId" runat="server" />
    </form>
    <script src="../../Scripts/airport.js" type="text/javascript"></script>
    <script src="/Scripts/json2.js" type="text/javascript"></script>
    <script src="../../Scripts/widget/common.js" type="text/javascript"></script>
    <script src="../../Scripts/Global.js?20121118" type="text/javascript"></script>
    <script src="/Scripts/OrganizationModule/CompanyGroup/Unit_Edit.aspx.js" type="text/javascript"></script>
</body>
</html>
