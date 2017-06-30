<%@ Page Title="Câu hỏi trắc nghiệm" Language="C#" MasterPageFile="~/Admin.Master" AutoEventWireup="true" CodeBehind="Question.aspx.cs" Inherits="BKTL.Question" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <style type="text/css">
        #dataTables-example_filter {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--Tiêu đề--%>
    <div class="row">
        <div class="col-lg-12 title-control-page">
            <h1 class="page-header">Quản lý Câu hỏi</h1>
            <button type="button" id="AddNew" data-toggle="modal" data-target="#InsertOrUpdate" class="btn btn-primary btnFunction" onclick="InsertOrUpdate(0,1,1,1,1,'','')">Thêm mới</button>
        </div>
    </div>
    <%--Bộ lọc--%>
    <div class="row">
        <div class="col-lg-12">
            <div class="panel-body" id="table-filter">
                <table style="width: 100% !important">
                    <tr>
                        <td style="width: 64px;">
                            <label class="control-label" for="txtQuestionContent" data-content="Mời bạn nhập gần đúng nội dung câu hỏi"
                                data-placement="top" data-toggle="popover" data-trigger="hover">
                                Nội dung:</label>
                        </td>
                        <td>
                            <input type="text" class="form-control" id="txtQuestionContent" placeholder="Nhập gần đúng" />
                        </td>
                        <td style="width: 102px">
                            <label class="control-label" for="cboChapter">Chương học:</label>
                        </td>
                        <td>
                            <select class="form-control" id="cboChapter">
                                <option value="1">Nguyễn Thị Hường</option>
                                <option value="2">Đặng Thị Quỳnh</option>
                                <option value="3">Lê Bảo Ngọc</option>
                            </select>
                        </td>
                        <td style="width: 71px">
                            <label class="control-label" for="cboLevel">Mức độ:</label>
                        </td>
                        <td>
                            <select class="form-control" id="cboLevel">
                                <option value="1">Nguyễn Thị Hường</option>
                                <option value="2">Đặng Thị Quỳnh</option>
                                <option value="3">Lê Bảo Ngọc</option>
                            </select>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%--Dữ liệu bảng--%>
    <div class="row">
        <div class="col-lg-12">
            <div class="panel panel-default">
                <div class="panel-body" id="table-content">
                    <table style="width: 100% !important" class='table table-striped table-bordered table-hover table-render-html' id='dataTables-example'>
                        <thead>
                            <tr>
                                <th class="th-code" data-content="Mã câu hỏi"
                                    data-placement="top" data-toggle="popover" data-trigger="hover">Mã CH</th>
                                <th>Nội dung câu hỏi</th>
                                <th>Chương học</th>
                                <th style="width: 75px">Mức độ</th>
                                <th style="width: 75px">Chức năng</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" id="hdQuestionID" />
</asp:Content>
