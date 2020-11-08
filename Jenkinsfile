node {
    checkout scm
    
    docker.image('mysql:5').withRun('-e "MYSQL_ROOT_PASSWORD=letmein2"') { c ->
        docker.image('maven:latest').inside("--link ${c.id}:db \
                                              -e 'DATABASE_USERNAME=root' \
                                              -e 'DATABASE_PASSWORD=letmein2' \
                                              -e 'DATABASE_DIALECT=org.hibernate.dialect.MySQL5Dialect' \
                                              -e 'DATABASE_URL=jdbc:mysql://db:3306/jenkinstest?createDatabaseIfNotExist=true'") {
            try {
                sh 'mvn package'
                junit 'target/surefire-reports/TEST-*.xml'                
            } finally {
            }                                            
        }
    }
}
