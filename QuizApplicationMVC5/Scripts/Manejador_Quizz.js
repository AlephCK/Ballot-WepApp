$(document).ready(function () {

    $('#SubmitQuiz').on('click', function () {

        //count Questions
        var sel = $('#countQuections').text();

        var resultQuiz = [], countQuestion = parseInt(sel), question = {}, j = 1;

        for (var i = 1; i < countQuestion; i++) {
            question = {
                QuestionID: $('#ID_Q' + i).text(),
                QuestionText: $('#Q' + i).text(),
                AnswerQ: $('input[name=inlineRadioOptions' + i + ']:checked').val()
            };

            resultQuiz.push(question);
        }

        $.ajax({

            type: 'POST',
            url: 'QuizTest',
            data: { resultQuiz },

            success: function (response) {

                if (response.result.length > 0) {
                    for (var i = 0; i < response.result.length; i++) {
                        if (response.result[i].isCorrect === true) {

                            $('#AnsQ' + j).html('<div class="alert alert-success" role="alert"><span class="glyphicon glyphicon-thumbs-up" aria-hidden="true"></span> Respuesta Correcta</div>');
                        }
                        else {
                            $('#AnsQ' + j).html('<div class="alert alert-danger" role="alert"> <span class="glyphicon glyphicon-thumbs-down" aria-hidden="true"></span> Respuesta Incorrecta - La Respuesta Correcta es <b>' + response.result[i].AnswerQ + '</b></div>');
                        }
                        j++;
                    }
                }
                else {
                    console.log("Something Wrong");
                }


                //console.log(response.result.length);

            },
            error: function (response) {

            }
        });

    });



});