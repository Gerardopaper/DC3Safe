$(document).ready(function () {
    createDataTable({
        tableId: "datatable",
        dataUrl: "/Trainers/List",
        buttonsId: "datatables-buttons",
        batchDeleteUrl: "/Trainers/BatchDelete",
        columns: [
            {
                data: 'id',
                render: function (data) {
                    return `
                            <input type="checkbox" class="form-check-input table-check" value="${data}" />
                            `;
                }
            },
            { data: 'name' },
            {
                data: 'id',
                render: function (data) {
                    return `
                                <a href="/Trainers/Edit/${data}" class="btn btn-sm btn-primary"><i class="fa-solid fa-pen"></i> Editar</a>
                                <a href="/Trainers/Delete/${data}" class="btn btn-sm btn-danger"><i class="fa-solid fa-trash"></i> Eliminar</a>
                           `;
                }
            }
        ],
        callback: function () {
            document.getElementById("dt-search-0").placeholder = "Nombre";
        }
    });
});