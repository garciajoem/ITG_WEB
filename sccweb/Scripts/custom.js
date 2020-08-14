//$(document).ready(function () {
    //$('#dtPagination').DataTable();
    //$('.dataTables_length').addClass('bs-select');
//});

$(document).ready(function () {
    //$('.selectpicker').selectpicker({
     //   style: 'btn-info',
     //   size: 4
    //});
});  

$(function () {
    var i = 1;
    var ri = "";

    $('.btnAddSubPage').click(function () {
        var menuId = $(this).data('menuid'), modal = $(this).data('modalmenu');


        $('#AddSubMenuItem').find('.submenuid').val(menuId);
        $('#AddSubMenuItem').find('.submenuparent').val(menuId);

        if (modal == 1) {
            $('#AddSubMenuItem .admin-bl-modal').show();
        } else {
            $('#AddSubMenuItem .admin-bl-modal').hide();
        }

        if (menuId == 1) {
            $('#AddSubMenuItem .menu-quick-links').show();
            $('#AddSubMenuItem .menu-default').hide();
        } else {
            $('#AddSubMenuItem .menu-quick-links').hide();
            $('#AddSubMenuItem .menu-default').show();
        }
    });

    $('.form-menu-create .selectpicker').on('change', function () {
        var optVal = $(this).find("option:selected").text();

        $(this).parents('.form-menu-create').find('#Name').val(optVal);
    });

    $('a[data-external="1"]').click(function (e) {
        var wlocation = $(this).attr('href');
        e.preventDefault();

        $('#modalRedirect').each(function () {
            $(this).fadeIn("fast");
            $(this).toggleClass('show');
            $(this).attr('aria-hidden', false);
            $(this).attr('aria-modal', true);
            $(this).find('#continue').attr('href', wlocation);
        });
    });

    $('#modalRedirect .cancel').click(function () {
        var modal = $(this).parents('#modalRedirect');

        $(modal).fadeOut();
        $(modal).toggleClass('show');
        $(modal).attr('aria-hidden', true);
        $(modal).attr('aria-modal', false);
    });

    $('.texteditor').each(function () {
        var textId = $(this).attr('id');
        CKEDITOR.replace(textId);
    });

    $('.form-page-create input[type="submit"], .form-page-edit input[type="submit"]').click(function (e) {
        var textlimit = 100;

        $('.texteditor:not(#Maintext)').each(function () {
            var textEditorId = $(this).attr('id'), ckEditorData = CKEDITOR.instances[textEditorId].getData(),
                dataReplace = ckEditorData.toString().replace(/\,/g, '&comma;');
            CKEDITOR.instances[textEditorId].setData(dataReplace);
        });

        $('input[name="subcontent"]').each(function () {
            var val = $(this).val(), dataReplace = val.toString().replace(/\,/g, '&comma;');

            $(this).val(dataReplace);
        });

        $('input[name="Summary"]').each(function () {
            if ($(this).val() != '') {
                $(this).val($(this).val().substring(0, textlimit));
            }
        });
    });

    $('#SubOptModalContent .btn-option-content').click(function () {
        var dataMenu = $(this).data('menutype');

        $('#ModalMenutype').val(dataMenu);

    });

    $('#SubOptModalContent input[type="submit"]').click(function (e) {
        var textlimit = 100;

        $('.texteditor:not(#Maintext)').each(function () {
            var textEditorId = $(this).attr('id'), ckEditorData = CKEDITOR.instances[textEditorId].getData(),
                dataReplace = ckEditorData.toString().replace(/\,/g, '&comma;');
            CKEDITOR.instances[textEditorId].setData(dataReplace);
        });

        $('input[name="ModalPanelContent"]').each(function () {
            var val = $(this).val(), dataReplace = val.toString().replace(/\,/g, '&comma;');

            $(this).val(dataReplace);
        });
    });

    $('.form-page-edit input[name="subcontent"]').each(function () {
        var val = $(this).val(), dataReplace = val.toString().replace(/\&comma;/g, ',');

        $(this).val(dataReplace);
    });

    $('.page-accordion .card-header h4').each(function () {
        var text = $(this).html(), dataReplace = text.toString().replace(/\&amp;comma;/g, ','), dataAndReplace = text.toString().replace(/\&amp;/g, '&');

        $(this).text(dataReplace);
        $(this).text(dataAndReplace);
    });

    $('.temptext').each(function () {
        var ptext = $(this).text(), dataAndReplace = ptext.toString().replace(/\&comma;/g, ',');;
        $(this).siblings('.card-body').append(dataAndReplace);
        $(this).remove();
    });

    $('.page .btn-add-mtext').click(function () {
        $('.form-multipletext').slideToggle();
    });

    $('.fi-data-img .fdi-remove').click(function () {
        $(this).parent().siblings('#upload').show();
        $(this).parent().remove();
    });

    $('.datepicker').datepicker({
        format: 'dd-mm-yy'
    });

    $('.page .ai-expand').click(function () {
        $(this).parents('.actions').siblings('.add-dtls').slideToggle();
        $(this).parents('tr').siblings('tr').toggleClass('overlay');
    });

    $('.textcontent').each(function () {
        var plaintext = $(this).text();
        $('.page-plaintext').append(plaintext);
        $(this).remove();
    });

    

    $('.add-dtls').each(function () {
        var pr = $(this).parents('.page').width();

        $(this).css('width', pr + 'px');
    });

    $('.btn-duplicate').click(function () {
        var container = $(this).data('container');   

        if ($(this).siblings(container).children('.fmi-sep:last-child').length == 1) {
            var dataLength = $(this).siblings(container).children('.fmi-sep:last-child').data('length');

            if (ri != "") {
                i = ri;
            } else {
                i = dataLength;
            }
        }

        i++;
        $(container).append('<div id="fmi-' + i + '" class="fm-item" data-fmitem="' + i + '"><input data-subid="' + i + '" type="text" name="subcontent" class="form-control fmi fmi-title" placeholder="Add Subtitle"/><textarea id="textEditor' + i + '" class="form-control texteditor fmi fmi-content" name="subcontent" data-conid="' + i + '" row="4" placeholder="Add Subcontent"></textarea><p class="text-right" style="margin:0; margin-top: 10px;"><span class="btn btn-sm btn-danger btn-remove-fmi">Remove</span></p></div>');
        CKEDITOR.replace('textEditor' + i);

        $('.btn-remove-fmi').click(function () {
            $(this).parents('.fm-item').remove();
        });
    });

    $('#modalDuplicate').click(function () {
        var container = $(this).data('container');

        if ($(this).siblings(container).children('.fmi-sep:last-child').length == 1) {
            var dataLength = $(this).siblings(container).children('.fmi-sep:last-child').data('length');

            if (ri != "") {
                i = ri;
            } else {
                i = dataLength;
            }
        }

        i++;
        $(container).append('<div id="fmi-' + i + '" class="fm-item" data-fmitem="' + i + '"><input data-subid="' + i + '" type="text" name="ModalPanelContent" class="form-control fmi fmi-title" placeholder="Add Subtitle"/><textarea id="textEditorModal' + i + '" class="form-control texteditor fmi fmi-content" name="ModalPanelContent" data-conid="' + i + '" row="3" placeholder="Add Subcontent"></textarea><p class="text-right" style="margin:0; margin-top: 10px;"><span class="btn btn-sm btn-danger btn-remove-fmi">Remove</span></p></div>');
        CKEDITOR.replace('textEditorModal' + i);

        $('.btn-remove-fmi').click(function () {
            $(this).parents('.fm-item').remove();
        });
    });

    $('.btn-remove-fmisep').click(function () {
        if ($('.container').children('.fmi-sep:last-child').length == 1) {
            var dataLength = $('.container').children('.fmi-sep:last-child').data('length');
            ri = dataLength;
        }
        var dataItem = $(this).data('remove');

        $(this).parents('.fmi').siblings('.fmi[data-item="' + dataItem + '"]').remove();
        $(this).parents('.fmi').remove();
    });

    //--- Start Button Toggle Class
    $('.btn-toggle').click(function () {
        var target = $(this).data('target'), toggleClass = $(this).data('toggle');
        $(target).toggleClass(toggleClass);
    });

    $('.btn.collapse-hidden').click(function () {
        var parentId = $(this).data('parentid'), toggleId = $(this).data('target');
        $(toggleId).attr('data-content', '1');
        $(parentId).attr('data-content', '0');
        $(parentId).slideToggle();
        $(toggleId).slideToggle();
    });
    $('.block.collapse-slide').click(function () {
        var parentId = $(this).data('parentid'), toggleId = $(this).data('toggleblock');
        

        if ($(this).hasClass('btn')) {
            $(parentId).toggleClass('show');
            $(toggleId).slideToggle();
            $('.admin-bl').attr('aria-expanded', 'false');
        } else {
            $(parentId).slideToggle();
        }
    });

    //--- End Button Toggle Class

    //--- Start Background Image Block
    $('.bg-img').each(function () {
        if ($(this).hasClass('bg-modal')) {
            var target = $(this).data('imgtarget'), imgsrc = $(this).find(target).attr('src');
        }
        else {
            var target = $(this).data('target'), imgsrc = $(this).find(target).attr('src');
        }

        $(this).attr('style', 'background-image: url(' + imgsrc + ')');
    });
    //--- End Background Image Block

    $('.page-content-main a').click(function (e) {
        var loc = $(this).attr('href'), linkDef = loc.indexOf('stclaircounty.org'), link = loc.indexOf('52.144.45.68');

        if (linkDef == -1 && link == -1) {
            e.preventDefault();

            $('#modalRedirect').each(function () {
                $(this).fadeIn("fast");
                $(this).toggleClass('show');
                $(this).attr('aria-hidden', false);
                $(this).attr('aria-modal', true);
                $(this).find('#continue').attr('target', '_blank');
                $(this).find('#continue').attr('href', loc);
            });
        }
    });
});