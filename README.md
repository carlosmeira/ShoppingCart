Projeto Carrinho de Compras:

Coloquei como primeiro objetivo deixar as classes o mais simples possivel. Não inclui campos que não influenciariam no
teste final das classes. Ex: Descrição de Produtos.

Optei por incluir uma classe estática para simular um database. Não ficou bom pois gerei uma dependencia circular no código, mas funciona para fins de teste.

Escondi o construtor da classe ShoppingCart para evitar que o programador instancie uma classe diretamente. O carrinho deve ser
gerado usando o método estático "Create", o qual vai verificar se já existe um carrinho iniciado no "DATABASE" e não finalizado para o cliente 
em questão, se existir reaproveita, caso contrário uma nova classe é instanciada. Poderia usar factory mas talvez seja overkill ou estaria desviando a real utilidade.

Implementados métodos padrão de CRUD para selecionar/adicionar/editar/remover itens no carrinho.

O método "AddItem" além de adicionar novos produtos também atualiza, pois no contexto de um carrinho de compras, se mais itens iguais são adicionados a quantidade é somada.

O método "UpdateItem" apenas atualiza a quantidade. Não dei atenção para atualização do valor unitário, pois isso virá de uma tabela de preços externa, o que vale é o valor que foi adicionado por primeiro.

O método "RemoveItem" retira o produto como um todo do carrinho e não uma quantidade, para isso pode ser utilizado o "AddItem" com uma quantidade negativa.

Os parametros "Valor Minimo = R$ 50" e "Quantidade máxima por produto = 10" poderiam ser configuráveis, deixaria os testes mais interessantes, além de ser uma funcionalidade válida para o sistema.

Incluida validação para quantidade mínima de itens.

O campo ShoppingCartId foi deixado igual ao ID do cliente, poderia ser random ou sequencial, não fiz pois não influencia os testes. 
Além do mais isso será tratado pelo database.
