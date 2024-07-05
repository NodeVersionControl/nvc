# Node Version Control (Controle de Versão do Node)

**Ferramenta de linha de comando .Net 8 para gerenciar versões do NodeJS*

Este aplicativo se inspira na popular ferramenta NVM (Node Version Manager).

  
Com este aplicativo, você poderá instalar, remover e alternar entre diferentes versões do NodeJS, tudo sem precisar usar privilégios de administrador. O NVC também manterá seus pacotes NPM globais em funcionamento com cada versão do NodeJS para permitir uma transição perfeita entre projetos.

Para que o NVC funcione corretamente, você precisará de privilégios de administrador para ajustar a variável PATH do sistema e, opcionalmente, armazenar o NVC no diretório \Program Files\ para acesso em qualquer lugar em uma janela cmd.

## Configurar

1. Clone o repositório em seu computador.

2. Construa o aplicativo em versão (é necessário ter Visual Studio 2022)

3. Copie a pasta Release NVC para \Program Files\ e adicione-a à variável PATH do seu sistema. Isto é para que você possa acessar o NVC de qualquer lugar na linha cmd/powershell (requer privilégios de administrador)

4. Edite a variável PATH do seu sistema para alterar o local da instalação do NodeJS.  A localização padrão do NVC será ``` C:\nodejs ```. Consulte a seção Configuração para alterar o local padrão.

5. Execute comandos usando nvc na linha de comando ou powershell.

## Comandos

### Listar todas as versões do NodeJS

* ``` nvc -l ```
* ``` nvc --list ```

### Mude manualmente para uma versão diferente do NodeJS.

* ``` nvc -c xx.xx.xx ```
* ``` nvc -c xx ``` *(Lida com strings parciais se for único)*
* ``` nvc --change xx.xx.xx ```


### Mude manualmente para uma versão diferente do NodeJS.

* ``` nvc -i xx.xx.xx ```
* ``` nvc --install xx.xx.xx ```

### Instalação automática através da lista de versões

* ``` nvc -r ```
* ``` nvc --release```

### Lista todas as versões instaladas do NodeJS. Selecione para alterar a versão

* ``` nvc -v ```
* ``` nvc --version```

### Remover uma versão do NodeJS

* ``` nvc -r xx.xx.xx ```
* ``` nvc -r xx ``` *(Lida com strings parciais se for único)*
* ``` nvc --remove xx.xx.xx ```


## Configuração

**Alterações na configuração podem ser feitas ajustando o arquivo config.json.**

### NODE_DIRECTORY

Diretório onde o node.exe será executado. Você precisará atualizar o caminho do ambiente do seu sistema para apontar para esta pasta também. **Certifique-se de que este diretório não exija privilégios de administrador para ler/gravar neste local.**

Valor padrão: ``` C:\nodejs ```

### NODE_VERSIONS_DIRECTORY

O diretório onde todas as versões de nós salvas serão armazenadas. **Certifique-se de que este diretório não exija privilégios de administrador para ler/gravar neste local.**

Valor padrão: ``` C:\nodejsVersions ```

### WINDOWS_ARCITECTURA

Arquitetura do NodeJS que você deseja instalar. Ou seja, x64 ou x86

Valor padrão: ``` x64 ```

### TEMP_FOLDER

Diretório para uma pasta Temp usada para baixar arquivos Zip. **Certifique-se de que aponta para uma pasta vazia, pois removerá regularmente todos os arquivos dessa pasta. Você foi avisado.**

Valor padrão: ``` %TEMP%\NodeJSVersionDownloads ```

### NPM_GLOBALS_DIRECTORY

Diretório onde o NPM instala seus pacotes globais. O valor padrão é o local da pasta padrão para NPM. Se você configurou o NPM para instalar pacotes globais em outro lugar, você precisará ajustar esse valor para corresponder.

Valor padrão: ``` %USERPROFILE%\AppData\Roaming\npm\node_modules ```
