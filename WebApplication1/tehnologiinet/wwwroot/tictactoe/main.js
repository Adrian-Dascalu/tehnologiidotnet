let turn = 0;

let win = 0;

let text_result = "";

var c = [0,0,0,0,0,0,0,0,0,0];

var text_turn = document.getElementById("text-turn");

text_turn.textContent = "X turn";

//include jquery
//var script = document.createElement('script');
//script.src = 'https://code.jquery.com/jquery-3.6.0.min.js';
//script.type = 'text/javascript';
// document.getElementsByTagName('head')[0].appendChild(script);

//var text_turn = $("#text-turn");
//text_turn.text("X turn");

let turnslogx = [];
let turnslogo = [];

var text_box;

let playerXUsername;
let playerOUsername;

let match_result_sent = false;

function loginX() {
    playerXUsername = document.getElementById("playerX").value;

    let playerXContainer = document.getElementById("playerXContainer");
    playerXContainer.innerHTML = `<div class="player-name" id="playerX">Player X: ${playerXUsername}</div>
        <button class="button-log-off" onclick="logOffX()">Log Off</button>`;
}

function loginO() {
    playerOUsername = document.getElementById("playerO").value;

    let playerOContainer = document.getElementById("playerOContainer");
    playerOContainer.innerHTML = `<div class="player-name" id="playerO">Player O: ${playerOUsername}</div>
        <button class="button-log-off" onclick="logOffO()">Log Off</button>`;
}

function logOffX() {
    let playerXContainer = document.getElementById("playerXContainer");
    
    playerXUsername = "";

    playerXContainer.innerHTML = `<div class="player-input" id="playerXContainer">
        <label for="playerX">Player X username:</label>
        <input type="text" id="playerX" placeholder="Enter X's name">

        <button class="button-log-in" onclick="loginX()">Login</button>
    </div>`;
}

function logOffO() {
    let playerOContainer = document.getElementById("playerOContainer");
    
    playerOUsername = "";

    playerOContainer.innerHTML = `<div class="player-input" id="playerOContainer">
        <label for="playerO">Player O username:</label>
        <input type="text" id="playerO" placeholder="Enter O's name">

        <button class="button-log-in" onclick="loginO()">Login</button>
    </div>`;
}

function restart() {
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

    match_result_sent = false;
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

        let winX = 0;
        let loseX = 0;
        let winO = 0;
        let loseO = 0;


        if (turn == 0) {
            winO = 1;
            loseO = 0;
            winX = 0;
            loseX = 1;
        }
        else {
            winX = 1;
            loseX = 0;
            winO = 0;
            loseO = 1;
        }

        if(match_result_sent == false) {
            $.ajax({
                url: "https://localhost:7241/TicTacToe/update-result",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    Username: playerOUsername,
                    Win: winO,
                    Lose: loseO,
                }),
                async: false
            })

            $.ajax({
                url: "https://localhost:7241/TicTacToe/update-result",
                type: "POST",
                contentType: "application/json",
                data: JSON.stringify({
                    Username: playerXUsername,
                    Win: winX,
                    Lose: loseX,
                }),
                async: false
            })

            match_result_sent = true;
        }
    }
}