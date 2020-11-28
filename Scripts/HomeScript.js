$(function () {
    var t = $("#onenxTable").DataTable({
        "language": {
            "search":"搜索",
            "lengthMenu": "每页 _MENU_ 条记录",
            "zeroRecords": "没有找到记录",
            "info": "第 _PAGE_ 页 ( 总共 _PAGES_ 页 )",
            "infoEmpty": "无记录",
            "infoFiltered": "(从 _MAX_ 条记录过滤)",
            "paginate": {
                first: "首页",
                previous: "前一页",
                next: "后一页",
                last: "尾页"
            },
            "emptyTable": "",
        }
    });

    $("#normal-form").validate(
        {
            onkeyup: function (element, event) {
                $(element).valid();
            },
            onfocusout: function (element, event) {
                $(element).valid();
            },
            rules: {
                "email-normal-form": { required: true, },
                "pwd-normal-form": { required: true, },
            },
            showErrors: function (errorMap, errorList) {
                var stutas = '<span class="glyphicon glyphicon-ok form-control-feedback"></span>';
                $.each(this.validElements(), function (index, element) {
                    var $element = $(element);
                    $element.data("title", "").removeClass("error").tooltip("destroy");
                    //$element.after($(stutas));
                    $element.closest(".form-group").addClass("has-success has-feedback");
                });

                $.each(errorList, function (index, error) {
                    var $element = $(error.element);
                    $element.tooltip("destroy").data("title", error.message).addClass("error").tooltip();
                });
            },
            messages: {
                "email-normal-form": {
                    required: "必填项",
                },
                "pwd-normal-form": {
                    required: "必填项",
                },

            },
            success: function (label) {
            }
        }
    );

    $("#txt_file").fileinput({
        dropZoneEnabled: false,
        language: 'zh', //设置语言
        //hideThumbnailContent: true, //
        showPreview: false,
        fileSizeGetter: true,
        //allowedFileExtensions: ['txt'],//接收的文件后缀
        uploadUrl: "/Home/Upload",
        //theme: "fa", // 主题设置
        showUpload: true, // 不显示上传按钮，选择后直接上传
        enctype: 'multipart/form-data',
    });

    //文件上传完成后更新界面
    $("#txt_file").on("fileuploaded", function (event, data, previewId, index) {
        var rowData = data.response;
        let computeHtml = '<button class="btn btn-primary" id="computeBtn" data-loading-text="Computing" onclick="Compute(this)">Compute</button>';
        let downloadHtml = data.SubrecipeGenStatus === "Success"
            ? '<button class="btn btn-success" id="downloadBtn" onclick="Download(this)">Download</button>'
            : '<button class="btn btn-success disabled" id="downloadBtn" disabled onclick="Download(this)">Download</button>'
        let newRow = [rowData.FileId, rowData.FileName, rowData.UploadDate, computeHtml, downloadHtml];
        t.row.add(newRow).draw();
        var datas = $("#onenxTable").find("td");
        $.each(datas, function (index, item) {
            if (item.innerText == rowData.FileId) {
                $(item).hide();
            }
        });
    });
});

function Compute(element) {
    var loading = $('<i class="fa fa-circle-o-notch fa-spin"></i> "');
    var $btn = $(element).button('Computing').prepend(loading);
    var tds = $(element).closest("tr").children(); 
    var downloadBtn = tds.find("#downloadBtn")[0];
    $(downloadBtn).removeClass("disabled").addClass("disabled").prop("disabled", true);
    var id = tds[0].innerText;
    var fileName = tds[1].innerText;
    $.ajax({
        method: "POST",
        url: "/Home/Compute",
        data: { id: id, fileName: fileName},
        success: function (data) {
            $(downloadBtn).removeClass("disabled").prop("disabled", false);
            $btn.closest("i").
            $btn.button('reset');
        },
        error: function (error) {
            $btn.css("background", "red");
        }
    });
}

function Download(element) {
    var id = $(element).closest("tr").children()[0].innerText; 
    window.location.href = "http://localhost:62013/home/download?id=" + id;
}