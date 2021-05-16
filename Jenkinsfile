node('docker-image-builder') {
    def app

    stage('Clone repository') {
        checkout scm
    }

    stage('Build image') {
       app = docker.build("ongoonku/conversation-planner")
    }

    stage('Test image') {
        app.inside {
            sh 'echo "Tests passed"'
        }
    }

    stage('Push image') {
        docker.withRegistry('https://registry.hub.docker.com', 'docker-ongoonku') {
            app.push("${env.BUILD_NUMBER}")
            app.push("latest")
        }
    }
}