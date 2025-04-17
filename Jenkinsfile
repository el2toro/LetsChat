pipeline {
    agent any
    environment {
        IMAGE_NAME = 'letschat'
        IMAGE_TAG = 'latest'
    }
    stages {
        stage('Publish App') {
            steps {
                script {
                    bat 'dotnet publish -c Release -o publish'
                }
            }
        }
        stage('Build Docker Image') {
            steps {
                script {
                    bat "docker build -t ${env.IMAGE_NAME}:${env.IMAGE_TAG} -f LetsChat/LetsChat/Dockerfile LetsChat"
                    bat "docker build -t %IMAGE_NAME%:%IMAGE_TAG% ."
                    bat "docker build -t %IMAGE_NAME%:%IMAGE_TAG% ."
                }
            }
        }
        stage('Run Docker Container') {
            steps {
                script {
                    // Stop old container if exists
                    bat "docker rm -f letschat-container || exit 0"
                    // Run new container
                    bat "docker run -d -p 8080:80 --name letschat-container ${env.IMAGE_NAME}:${env.IMAGE_TAG}"
                }
            }
        }
    }
    post {
        always {
            bat 'docker container prune -f'
            bat 'docker image prune -f'
        }
    }
}
