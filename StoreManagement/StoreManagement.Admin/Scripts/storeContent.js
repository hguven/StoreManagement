﻿


$(document).ready(function () {
    console.log("jquery is workingg");



    try {
        $("#contentTreeview").bind("select_node.jstree", function (event, data) {
            var m = $("#" + data.selected[0]).find("[data-category-item]");
            var selectedCategoryId = m.first().attr("data-category-item");
            console.log(selectedCategoryId);
            $("#CategoryId").val(selectedCategoryId);

            $("#ProductCategoryId").val(selectedCategoryId);

        });
    }
    catch (err) {
        console.error(err.message);
    }


    try {
        
        if ($("#CategoryId").length) {
            GetCategoryTree($("#StoreId").val(), $("#categoryType").val());
        }
    }
    catch (err) {
        console.error(err.message);
    }


    try {

        if ($("#ProductCategoryId").length) {
            GetProductCategoryTree($("#StoreId").val(), $("#categoryType").val());
        }

    }
    catch (err) {
        console.error(err.message);
    }


    try {
        if ($("#StoreId").length) {
            populateStoreLabelsDropDown($("#StoreId").val());
        }
    }
    catch (err) {
        console.error(err.message);
    }

 

    try {

        $('select#StoreDropDownId').select2({}).change(function (event) {


         

            try {
                
                if ($("#CategoryId").length) {
                    GetCategoryTree($(this).val(), $("#categoryType").val());
                }

            }
            catch (err1) {
                console.error(err1.message);
            }

            try {


                if ($("#ProductCategoryId").length) {
                    GetProductCategoryTree($(this).val(), $("#categoryType").val());
                }



            }
            catch (err2) {
                console.error(err2.message);
            }



            try {

                populateStoreLabelsDropDown($(this).val());
            }
            catch (err4) {
                console.error(err.message);
            }



            console.log("deneme");
        });
        
    }
    catch (err) {
        console.error(err.message);
    }



  

    console.log("jquery is working");

    //GetFiles($("#Id").val());


    try {
      

        $('textarea[data-html-editor="normal"]').ckeditor({
            height: '360px',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                { name: 'basicstyles', items: ['Bold', 'Italic'] }
            ]
        });

        $('textarea[data-html-editor="full"]').ckeditor({
            height: '360px',
            toolbar: [
                { name: 'document', items: ['Source', '-', 'NewPage', 'Preview', '-', 'Templates'] }, // Defines toolbar group with name (used to create voice label) and items in 3 subgroups.
                ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'], // Defines toolbar group without name.
                { name: 'basicstyles', items: ['Bold', 'Italic'] }
            ]
        });
        
    }
    catch (err) {
        console.error(err.message);
    }




});

function GetCategoryTree(id, categoryType) {

    var jsonRequest = JSON.stringify({ "storeId": id, "categoryType": categoryType });
    console.log(jsonRequest);

    jQuery.ajax({
        url: "/Ajax/GetCategoriesTree",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#categoryTree").html(data);
            bindcategoryTree();
            var categoryId = $("#CategoryId").val();
            var selectedCategory = $('[data-category-item=' + categoryId + ']').text();
            $('[data-category-item=' + categoryId + ']').attr("class", "btn btn-danger");
            console.log(selectedCategory);
            $("#SelectedCategory").text(selectedCategory);
            bindCategoryRelatedItemsCount($("#entityType").val(), 'data-category-item-item');
        },
        error: function (request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function () {

        }
    });

}

function bindcategoryTree() {
    $('[data-category-item]').each(function () {
        $(this).off("click");
        $(this).on("click", handleCategoryTree);
    });
}

function handleCategoryTree(e) {
    var caller = e.target;
    var categoryId = $(caller).attr('data-category-item');
    var category = $(caller).text();


    $('[data-category-item]').each(function () {
        $(this).attr("class", "btn btn-link");
    });

    $(caller).attr("class", "btn btn-danger");



    if ($("#SelectedCategory").length) {
        $("#SelectedCategory").text(category);
    }

    if ($("#CategoryId").length) {
        $("#CategoryId").val(categoryId);
    }

    if ($("#ProductSelectedCategory").length) {
        $("#ProductSelectedCategory").text(category);
    }

    if ($("#ProductCategoryId").length) {
        $("#ProductCategoryId").val(categoryId);
    }


}

//////////////////////////////////////////////////////////////////////////////
function GetProductCategoryTree(id, categoryType) {

    var jsonRequest = JSON.stringify({ "storeId": id, "categoryType": categoryType });
    console.log(jsonRequest);

    jQuery.ajax({
        url: "/Ajax/GetProductCategoriesTree",
        type: 'POST',
        data: jsonRequest,
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $("#productCategoryTree").html(data);
            bindcategoryTree();
            var categoryId = $("#ProductCategoryId").val();
            $('[data-category-item=' + categoryId + ']').attr("class", "btn btn-danger");
            var selectedCategory = $('[data-category-item=' + categoryId + ']').text();
            console.log($('[data-category-item=' + categoryId + ']'));
            $("#ProductSelectedCategory").text(selectedCategory);
            bindCategoryRelatedItemsCount($("#entityType").val(), 'data-category-item');
        },
        error: function (request, status, error) {
            console.error('Error ' + status + ' ' + request.responseText);
        },
        beforeSend: function () {

        }
    });

}


