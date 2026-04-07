grammar ArrayInit;

init : '{' value (',' value)* '}' ;

value
    : INT
    | init
    ;

INT : [0-9]+ ;
WS : [ \t\r\n]+ -> skip ;