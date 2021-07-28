# TechTips Wavenet : Créer son œuvre d’art numérique

Le 30 juillet, Simon Baudart nous proposera un Tech-Tips d’un nouveau genre :

**Créer son oeuvre d’art numérique**

*Ou, comment créer une école de dessin quand on ne sait pas dessiner mais qu’on sait coder ?!?**

## Au programme :
-	MicroServices
-	Docker
-	Azure Service Bus

## Vidéo de la présentation
- A venir !

# Aide mémoire de la présentation
1. Création d'un générateur de NFT : Explication du but
    - https://foundation.app/
2. Structure : Expliquer les approches
    - Notion de Command et de Fact : Service Bus, Event Bus, ... https://youtu.be/sTwZwu1tLek?t=422
    - MicroServices et Docker
3. Création du service bus et de l'explorer : https://github.com/paolosalvatori/ServiceBusExplorer
    - Pricing
    - Création de la queue
        - Message Time To LLive
        - Message Lock Duration
4. Création du Mentor
    - Notions de MicroService
    - Notions de HostedServices
        - Console.WriteLine
    - Push to the queue
        - Envoyer un message texte
            - Le voir dans la queue
        - Envoyer un objet dans la queue
5. Création du Docker File
    - Explication
    - docker build . -f Acme.Demo.MicroServices.DrawingMentor\Dockerfile -t acme/demo/mentor:latest --target final
    - docker image list
    - docker run -d acme/demo/mentor:latest
    - docker container list
    - docker container attach xxxxx
6. Création du Drawer Simple
    - Aussi un micro service
    - Sauvegarde dans Azure Storage
        - Récupération des fichiers
    - Notion de AbandonMessage !!!
    - Publication de l'image
7. Création du Drawer Advanced
    - Idem mais advanced :)
    - Publication de l'image
8. More Powerful !!! Wavenet Drawers !
    - Présentation du projet
    - Création de l'image
    - On est plusieurs chez Wavenet !!! Et on a du CPU et de la RAM !
9. Conclusion et questions :)