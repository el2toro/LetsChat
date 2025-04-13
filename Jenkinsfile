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
                    // Publish .NET Core Application
                    bat 'dotnet publish -c Release -o ./publish'
                }
            }
        }
        stage('Build Docker Image') {
            steps {
                script {
                    // Build Docker image
                    bat 'docker build -t $IMAGE_NAME:$IMAGE_TAG .'
                }
            }
        }
        stage('Run Docker Container') {
            steps {
                script {
                    // Run Docker container
                    bat 'docker run -d -p 8080:80 $IMAGE_NAME:$IMAGE_TAG'
                }
            }
        }
    }
    post {
        always {
            // Clean up Docker containers and images after the build
            bat 'docker container prune -f'
            bat 'docker image prune -f'
        }
    }
}
