var Global_Url = '';


function serializeForm(frm) {
    var data = $(frm).serializeArray();
    var formData = {};
    var hasError = false; // Variable para verificar si hay errores

    for (var i = 0; i < data.length; i++) {
        try {
            var $currentCtrl = $(frm).find("[name='" + data[i].name + "']");
            if ($currentCtrl.length != 0) {
                if ($currentCtrl.is('input[type="checkbox"]')) {
                    data[i].value = $currentCtrl.is(':checked') ? "true" : "false";
                }

                if ($currentCtrl.is('select')) {
                    if ($currentCtrl.prop('required') && !$currentCtrl.val()) {
                        hasError = true; // Se encontró un campo requerido vacío
                        $currentCtrl.css('box - shadow', ' 0 0 2px white inset, 0 1px 0 #bb9a6261'); // Resaltar el borde del campo faltante en rojo
                    } else {
                        $currentCtrl.css('box - shadow', ''); // Restablecer el estilo del borde si el campo está completo
                    } b

                }
                else if ($currentCtrl.is('input')) {
                    // Verificar campos requeridos
                    if ($currentCtrl.prop('required') && !data[i].value) {
                        hasError = true; // Se encontró un campo requerido vacío
                        $currentCtrl.css('box - shadow', ' 0 0 2px white inset, 0 1px 0 #bb9a6261'); // Resaltar el borde del campo faltante en rojo
                    } else {
                        $currentCtrl.css('box - shadow', ''); // Restablecer el estilo del borde si el campo está completo
                    }
                }
            }
        } catch (e) {
            console.error(e);
        }
        formData[data[i].name] = data[i].value;
    }

    if (hasError) {
        // Aquí puedes realizar alguna acción adicional en caso de que haya campos faltantes
        console.log('Error: Algunos campos requeridos no han sido completados.');
    }

    return {
        formData: formData,
        hasError: hasError
    };
}