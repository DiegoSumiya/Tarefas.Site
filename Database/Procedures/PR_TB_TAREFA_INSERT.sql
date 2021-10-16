CREATE PROCEDURE PR_TB_TAREFA_INSERT
(
	@DATA DATE,
	@DESCRICAO VARCHAR(100),
	@NOTIFICACAO BIT,
	@IDCATEGORIA INT,
	@EMAIL_USUARIO VARCHAR(100)
)
AS
BEGIN
	INSERT INTO TAREFA (DATA, DESCRICAO, NOTIFICACAO, IDCATEGORIA, EMAIL_USUARIO)
	VALUES (@DATA, @DESCRICAO, @NOTIFICACAO, @IDCATEGORIA, @EMAIL_USUARIO);

	SELECT SCOPE_IDENTITY();

END



