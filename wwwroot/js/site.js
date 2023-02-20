function readURL(input, a) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(a).attr('src', e.target.result).width(350).height(300);
        };
        reader.readAsDataURL(input.files[0]);
    }
}
function readURL1(input,a) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(a).attr('src', e.target.result).width(200).height(200);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function readURL2(input,a) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(a).attr('src', e.target.result).width(250).height(210);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function readURL3(input, a) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(a).attr('src', e.target.result).width().height();
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function getValue(id) {
    var select = document.getElementById(id);
    var value = select.options[sel.selectedIndex].value;
    alert(value);
}
function getPrice(a, input) {
    let num1 = a.value;
    let num2 = input.val();
    console.log(num1, num2, typeof num1, typeof num2);
    const answer = num1 + num2;
    $('#test').text(answer);
}

$('#myModal').on('shown.bs.modal', function () {
    $('#myInput').trigger('focus')
})
