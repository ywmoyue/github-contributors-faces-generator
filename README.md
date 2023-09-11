# Github contributors 头像图片生成器

类似 opencollective 的 [readme-integration](https://docs.opencollective.com/help/contributing/development/readme-integration) 

生成一张指定 Github 仓库的 contributors 头像图片

由于找不到方法把fork仓库加入到 opencollective 中，所以自己写一个

## 参数
指定仓库，必填
```
-r user/repoName
--repo user/repoName
```
模式，可选png/html，默认png, 非必填
```
-m png
--mode html
```
输出路径，非必填，默认output.png
```
-o ./test/test.png
--output ./test/test.png
```
令牌，github访问令牌，非必填，报错403后才需要填写
```
-s github_token
--secret github_token
```
宽度，非必填，默认860
```
-w 860
--width 860
```