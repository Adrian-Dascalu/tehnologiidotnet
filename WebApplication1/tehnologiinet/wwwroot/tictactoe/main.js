let turn = 0;

let win = 0;

let text_result = "";

var c = [0,0,0,0,0,0,0,0,0,0];

var text_turn = document.getElementById("text-turn");

text_turn.textContent = "X turn";

let turnslogx = [];
let turnslogo = [];

var text_box;

function restart() {
    let playerXInput = document.getElementById("playerX");
    let playerOInput = document.getElementById("playerO");

    if (!playerXInput || !playerOInput) {
        console.error("Input elements not found!");
        return;
    }

    let playerX = playerXInput.value.trim();
    let playerO = playerOInput.value.trim();

    if (playerX === "") playerX = "Player X";
    if (playerO === "") playerO = "Player O";

    // Update the display text without using innerHTML
    let playerXContainer = document.getElementById("playerXContainer");
    let playerOContainer = document.getElementById("playerOContainer");

    playerXContainer.textContent = `X: ${playerX}`;
    playerOContainer.textContent = `O: ${playerO}`;

    win = 0;
    c = [0,0,0,0,0,0,0,0,0,0];
    text_turn.textContent = "X turn";
    turn = 0;
    
    text_match_result = document.getElementById("result");
    text_match_result.textContent = "";

    for(i = 1; i <= 9; i++) {
        text_box = document.getElementById("box-" + i);
        text_box.textContent = "";

        text_box = document.getElementById("box2-" + i);
        text_box.textContent = "";
    }

    text_box = document.getElementById("box1-" + turnslogo[0]);
    text_box.textContent = "";

    text_box = document.getElementById("box1-" + turnslogx[0]);
    text_box.textContent = "";
    
    turnslogx = [];
    turnslogo = [];
}

function check3turns(x) {
    if(x == 0) {
        if(turnslogo.length > 3) {
            c[turnslogo[0]] = 0;

            text_box = document.getElementById("box-" + turnslogo[0]);
            text_box.textContent = "";

            text_box = document.getElementById("box1-" + turnslogo[0]);
            text_box.textContent = "";

            turnslogo.shift();     

            text_box = document.getElementById("box-" + turnslogo[0]);
            text_box.textContent = "";
            
            text_box = document.getElementById("box1-" + turnslogo[0]);
            text_box.textContent = "O";
        }
        else if(turnslogo.length == 3) {
            text_box = document.getElementById("box-" + turnslogo[0]);
            text_box.textContent = "";

            text_box = document.getElementById("box1-" + turnslogo[0]);
            text_box.textContent = "O";
        }
    }
    else {
        if(turnslogx.length > 3) {
            c[turnslogx[0]] = 0;

            text_box = document.getElementById("box-" + turnslogx[0]);
            text_box.textContent = "";
            
            text_box = document.getElementById("box1-" + turnslogx[0]);
            text_box.textContent = "";

            turnslogx.shift();

            text_box = document.getElementById("box-" + turnslogx[0]);
            text_box.textContent = "";

            text_box = document.getElementById("box1-" + turnslogx[0]);
            text_box.textContent = "X";
        }
        else if(turnslogx.length == 3) {
            text_box = document.getElementById("box-" + turnslogx[0]);
            text_box.textContent = "";

            text_box = document.getElementById("box1-" + turnslogx[0]);
            text_box.textContent = "X";
        }
    }
}

function calcul(v) {
    if (win == 0) {
       
        text_box = document.getElementById("box-" + v);

        if (c[v] == 0)
        {
            if (turn == 0) {
                text_box.textContent = "X";

                turn = 1;

                c[v] = 1;

                text_turn.textContent = "O turn";

                turnslogx.push(v);

                check3turns(1);
            }
            else {
                text_box.textContent = "O";

                turn = 0;

                c[v] = 2;

                text_turn.textContent = "X turn";

                turnslogo.push(v);

                check3turns(0);
            }

            if (((c[1] == c[2]) && (c[1] == c[3]) && (c[1] != 0)) || ((c[4] == c[5]) && (c[4] == c[6]) && (c[4] != 0)) ||
                ((c[7] == c[8]) && (c[7] == c[9]) && (c[7] != 0)) || ((c[1] == c[4]) && (c[1] == c[7]) && (c[1] != 0)) ||
                ((c[2] == c[5]) && (c[2] == c[8]) && (c[2] != 0)) || ((c[3] == c[6]) && (c[3] == c[9]) && (c[3] != 0)) ||
                ((c[1] == c[5]) && (c[1] == c[9]) && (c[1] != 0)) || ((c[3] == c[5]) && (c[3] == c[7]) && (c[3] != 0))) {
                
                win = 1;
                
                if (turn == 0) {
                    text_result = "O is the great chicken winner!";
                    text_turn.textContent = "End";

                    for(i = 1; i <= 9; i++) {
                        if(c[i] == 2) {
                            text_box = document.getElementById("box-" + i);
                            text_box.textContent = "";

                            text_box = document.getElementById("box1-" + i);
                            text_box.textContent = "";

                            text_box = document.getElementById("box2-" + i);
                            text_box.textContent = "O";
                        }
                    }
                }
                else {
                    text_result = "X is the great chicken winner!";
                    text_turn.textContent = "End";

                    for(i = 1; i <= 9; i++) {
                        if(c[i] == 1) {
                            text_box = document.getElementById("box-" + i);
                            text_box.textContent = "";

                            text_box = document.getElementById("box1-" + i);
                            text_box.textContent = "";

                            text_box = document.getElementById("box2-" + i);
                            text_box.textContent = "X";
                        }
                    }
                }
            }
        }
    }

    if (win == 1) {
        text_match_result = document.getElementById("result");

        text_match_result.textContent = text_result;

        fetch("https://localhost:7241/TicTacToe/update-result", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                studentId: 1,
                won: 1
            })
        })
        .then(response => response.json())
        .then(data => console.log("Updated:", data))
        .catch(error => console.error("Error updating result:", error));
    }
}