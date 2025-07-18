﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#categorytable').DataTable({
        "ajax": { url: '/admin/category/getall' },
        "columns": [
            { data: 'name', "width": "25%" },
            { data: 'displayOrder', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                            <a href="/admin/category/upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i> Edit </a>
                             <a onClick=Delete('/admin/category/delete?id=${data}') class="btn btn-danger mx-2"><i class="bi bi-trash-fill"></i> Delete </a>
                            </div>`
                },
                "width": "25%"
            }
        ]
    });
}


function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You want to delete this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "OK"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}