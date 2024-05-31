$(() => {
   
    $("#signup").on('click', function () {
        const email = $("#email").val();
        const password = $("#password").val();

        if (!email || !password) {
            $(".btn-lg").prop('disabled')

        }
    
    });
})