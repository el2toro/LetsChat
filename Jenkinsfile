pipeline {
    agent any
    environment {
        IMAGE_NAME = 'letschat'
        IMAGE_TAG = 'latest'
        SQL_CONTAINER_NAME = 'sqlserver'
        SQL_PASSWORD = 'YourPassword123!'
        DOCKER_NETWORK = 'letschat-net'
    }
    stages {

        stage('Create Docker Network') {
            steps {
                script {
                    bat "docker network create ${DOCKER_NETWORK} || exit 0"
                }
            }
        }

        stage('Start SQL Server Container') {
            steps {
                script {
                    bat """
                        docker run -d --name ${SQL_CONTAINER_NAME} --network ${DOCKER_NETWORK} ^
                        -e \"ACCEPT_EULA=Y\" -e \"SA_PASSWORD=${SQL_PASSWORD}\" ^
                        -p 1433:1433 mcr.microsoft.com/mssql/server:2022-latest
                    """
                }
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Run Tests') {
            steps {
                bat """
                    dotnet test --logger:\"trx;LogFileName=test_results.trx\" --results-directory:\"TestResults\"
                """
            }
        }

        stage('Publish App') {
            steps {
                bat 'dotnet publish LetsChat/LetsChat.csproj -c Release -o publish'
            }
        }

        stage('Build Docker Image') {
            steps {
                bat "docker build -t ${IMAGE_NAME}:${IMAGE_TAG} -f LetsChat/Dockerfile ."
            }
        }

        stage('Run LetsChat App') {
            steps {
                script {
                    // Remove existing app container if needed
                    bat "docker rm -f letschat-container || exit 0"

                    // Run the app container with same network
                    bat """
                        docker run -d --name letschat-container --network ${DOCKER_NETWORK} ^
                        -p 9090:8080 -e \"ConnectionStrings__DefaultConnection=Server=${SQL_CONTAINER_NAME};Database=LetsChatDb;User Id=sa;Password=${SQL_PASSWORD};\" ^
                        ${IMAGE_NAME}:${IMAGE_TAG}
                    """
                }
            }
        }
    }

    post {
        always {
            bat "docker rm -f letschat-container || exit 0"
            bat "docker rm -f ${SQL_CONTAINER_NAME} || exit 0"
            bat "docker network rm ${DOCKER_NETWORK} || exit 0"
            cleanWs()
        }
    }
}
