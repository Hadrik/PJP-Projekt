grammar Language;

/** The start rule; begin parsing here. */
program: statement+ ;

statement
    : ';'                                               # empty
    | primitiveType IDENTIFIER (',' IDENTIFIER)* ';'    # declaration
    | expression ';'                                    # evaluation
    | READ IDENTIFIER (',' IDENTIFIER)* ';'             # read
    | WRITE expression (',' expression)* ';'            # write
    | '{' statement* '}'                                # block
    | IF '(' expression ')' statement (ELSE statement)? # ifElse
    | WHILE '(' expression ')' statement                # while
    ;

expression
    : SUB expression                                   # unaryMinus
    | NOT expression                                   # logicalNot
    | expression op=(MUL|DIV|MOD) expression           # multiplicative
    | expression op=(ADD|SUB|CONCAT) expression        # additive
    | expression op=(LT|GT) expression                 # relation
    | expression op=(EQ|NEQ) expression                # comparison
    | expression op=AND expression                     # logicalAnd
    | expression op=OR expression                      # logicalOr
    | <assoc=right> IDENTIFIER '=' expression          # assignment
    | atom                                             # atomExpr
    ;

atom
    : INT                                              # int
    | FLOAT                                            # float
    | BOOL                                             # bool
    | STRING                                           # string
    | IDENTIFIER                                       # id
    | '(' expression ')'                               # parens
    ;

primitiveType
    : type=INT_KW
    | type=FLOAT_KW
    | type=BOOL_KW
    | type=STRING_KW
    ;


INT_KW : 'int';
FLOAT_KW : 'float';
BOOL_KW : 'bool';
STRING_KW : 'string';

INT : [0-9]+ ;
FLOAT : [0-9]+'.'[0-9]+ ;
BOOL : 'true' | 'false' ;
STRING : '"' .*? '"' ;

ADD : '+' ;
SUB : '-' ;
MUL : '*' ;
DIV : '/' ;
MOD : '%' ;
LT: '<' ;
GT: '>' ;
EQ: '==' ;
NEQ: '!=' ;
AND : '&&' ;
OR : '||' ;
NOT : '!' ;
CONCAT : '.' ;

READ : 'read' ;
WRITE : 'write' ;
IF : 'if' ;
ELSE : 'else' ;
WHILE : 'while' ;

WS : [ \t\r\n]+ -> skip ;
COMMENT : '//' ~[\r\n]* -> skip ;
IDENTIFIER : [a-zA-Z_][a-zA-Z0-9_]* ;
