FROM tomcat:9.0

COPY /target/*.war /usr/local/tomcat/webapps

# make the app war the root war so all default requests are directed to it
RUN mv /usr/local/tomcat/webapps/demo-1.0.0.war /usr/local/tomcat/webapps/ROOT.war
RUN mv /usr/local/tomcat/webapps/ROOT /usr/local/tomcat/webapps/ROOT_OLD

# run tomcat
CMD ["/usr/local/tomcat/bin/catalina.sh", "run"]
