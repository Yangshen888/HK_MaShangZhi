<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.demo.query.totalCount"
        :pageSize="stores.demo.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="10">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Select
                      v-model="stores.demo.query.kw"
                      :filterable="false"
                      :clearable="true"
                      style="width:150px;"
                      placeholder="请选择流程"
                      @on-change="handleSearchDispatch()"
                    >
                      <Option
                        v-for="item in stores.demo.sources.flowList"
                        :value="item.value"
                        :key="item.value"
                      >{{ item.label }}</Option>
                    </Select>
                  </FormItem>
                  <FormItem>
                    <Date-picker type="date" placeholder="选择日期" 
                      @on-change="handleSearchDispatch()" v-model="stores.demo.query.kw1"></Date-picker>
                  </FormItem>
                </Form>
              </Col>
              <Col span="14" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                >添加</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.demo.data"
          :columns="stores.demo.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button type="error" v-can="'deletes'" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="流程" prop="flowName">
            <Select
              :filterable="false"
              v-model="formModel.fields.flowName"
              style="width:100%"
              placeholder="请选择流程"
              :disabled="checkShow()"
            >
              <Option
                v-for="item in stores.demo.sources.flowList"
                :value="item.value"
                :key="item.value"
              >{{ item.label }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="时间" prop="flowTime">
            <DatePicker
              v-model="formModel.fields.flowTime"
              @on-change="formModel.fields.flowTime=$event"
              format="yyyy-MM-dd"
              type="date"
              placeholder="请选择时间"
              style="width: 100%"
              :disabled="checkShow()"
              :options="options"
            ></DatePicker>
          </FormItem>
        </Row>
        </Row>
        <Row>
          <div class="demo-upload-list" v-for="(item,index) in uploadList" :key="index">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleView(item.url)"></Icon>
                <Icon type="ios-trash-outline" @click.native="handleRemove(item.name)" v-if="!checkShow()"></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
          <Divider dashed />
          <Upload
            ref="upload"
            :show-upload-list="false"
            :default-file-list="defaultList"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :format="['jpg','jpeg','png']"
            :max-size="5120"
            :data="{uuid:this.$store.state.user.userGuid}"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            :headers="postheaders"
            type="drag"
            :action="actionurl"
            style="display: inline-block;width:58px;"
            v-if="!checkShow()"
          >
            <div style="width: 58px;height:58px;line-height: 58px;">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
        
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable"
          v-if="!checkShow()"
        >保 存</Button>
        <Button style="margin-left: 30px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  Getlist, //显示列表
  GetCreate, //新增
  GetShow, //获取选定信息
  batchCommand, //批量删除
  deleteDepartment, //单个删除
  GetEdit, //编辑
  deletetoFile,
  getRegistPicture
} from "@/api/process/flow";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "flow",
  components: {
    DzTable,
  },
  data() {
    return {
      options: {
        disabledDate(date) {
          return date && date.valueOf() > Date.now();
        },
      },
      url: config.baseUrl.dev,
      //附件上传
      imgshow1: false,
      img1: "",
      img: [],
      uploadList: [],
      defaultList: [],
      actionurl: "",
      postheaders: "",
      imgName: "",
      loadingStatus: false,
      updisabled: false,
      visible: false,
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
      },
      stores: {
        demo: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw1: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            flowList: [
              // {
              //   value: "采购",
              //   label: "采购",
              // },
              {
                value: "验收",
                label: "验收",
              },
              {
                value: "清洗",
                label: "清洗",
              },
              {
                value: "切配",
                label: "切配",
              },
              {
                value: "加工",
                label: "加工",
              },
              {
                value: "成菜",
                label: "成菜",
              },
            ],
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "flowUuid" },
            { title: "流程", key: "flowName",},
            { title: "时间", key: "flowTime",},
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        fields: {
          flowName: "",
          flowTime: "",
          Accessory: "",
          flowUuid: "",
        },
        rules: {
          flowName: [{  required: true, message: "请选择流程",trigger:'blur' }],
          flowTime: [{  required: true, message: "请选择时间",trigger:'blur'}],
        },
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑信息";
      }
      if (this.formModel.mode === "show") {
        return "信息详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.flowUuid);
    }, //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
      Getlist(this.stores.demo.query).then((res) => {
        this.stores.demo.data = res.data.data;
        this.stores.demo.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.demo.query.currentPage = page;
      this.loadDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.demo.query.pageSize = pageSize;
      this.loadDispatchList();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadDispatchList();
    },
    //刷新
    handleRefresh() {
      this.loadDispatchList();
    },
    checkShow() {
      return this.formModel.mode === "show";
    },
    //详情显示
    handleDetail(e) {
      this.formModel.mode = "show";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(e.flowUuid);
    },
    //添加按钮
    handleShowCreateWindow() {
      this.formModel.mode = "create";
      this.handleResetFormDispatch(); //清空表单
      this.formModel.opened = true;
    },
    //右边编辑
    async handleEdit(row) {
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(row.flowUuid);
    },
    //查询当前行信息
    doLoadData(id) {
      GetShow(id).then((res) => {
        if (res.data.code === 200) {
          this.formModel.fields = res.data.data;
          if (res.data.data.accessory != null) {
            let list = res.data.data.accessory.split(",");
            for (let i = 0; i < list.length; i++) {
              this.uploadList.push({
                url:
                  config.baseUrl.dev + "UploadFiles/RegistPicture/" + list[i],
                status: "finished",
                name: "UploadFiles/RegistPicture/" + list[i],
                fileName: list[i]
              });
            }
          }
        }
      });
    },
    //保存按钮
    handleSubmitConsumable() {
        console.log(this.formModel.fields.flowName)
      if (this.formModel.fields.flowName == null || this.formModel.fields.flowName == "") {
        this.$Message.warning("请选择流程类型!");
        return;
      }
      if(this.uploadList.length<=0){
        this.$Message.warning("请上传图片!");
        return;
      }
      if (this.formModel.fields.flowTime == "") {
        this.$Message.warning("请选择时间!");
        return;
      }
      if (this.formModel.mode === "create") {
        this.docreateDispatch();
      }
      if (this.formModel.mode === "edit") {
        this.doEditDispatch();
      }
    },
    //添加（保存）
    docreateDispatch() {
      GetCreate(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doEditDispatch() {
      if (
        this.formModel.fields.flowTime != "" &&
        this.formModel.fields.flowTime != null
      ) {
        this.formModel.fields.flowTime = new Date(
          Date.parse(new Date(this.formModel.fields.flowTime)) + 8 * 3600 * 1000
        );
      }
      GetEdit(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
      this.formModel.fields.accessory = "";
      this.uploadList=[];
    },
    //右边删除（单个删除）
    handleDelete(row) {
      this.doDelete(row.flowUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteDepartment(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //右上边删除按钮
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.selection = [];
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    async showUpResult(response, file, fileList) {
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        if (this.formModel.fields.accessory != null) {
          if (this.formModel.fields.accessory.length > 0) {
            this.formModel.fields.accessory += ",";
          }
          this.formModel.fields.accessory += response.data.fname;
        } else {
          this.formModel.fields.accessory = response.data.fname;
        }
        await this.uploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.fname
        });
      } else {
        this.$Message.warning(response.message);
      }
    },
    toUpResult() {
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件: " + file.name + " 不是png,jpg"
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件 " + file.name + " 太大,超过5M."
      });
    },
    handleBeforeUpload() {
      // const check = this.uploadList.length < 5;
      // if (!check) {
      //   this.$Notice.warning({
      //     title: "Up to five pictures can be uploaded."
      //   });
      // }
      // return check;
      return true;
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
      console.log(this.imgName);
    },
    handleRemove(file) {
      console.log(file);
      // const fileList = this.$refs.upload.fileList;
      // this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
      deletetoFile({ path: file }).then(res => {
        console.log(res);
        if (res.data.data == "200") {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join(",");
        } else {
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.accessory = this.uploadList
            .map(x => x.fileName)
            .join(",");
        }
      });
    }
  },
  mounted() {
    this.loadDispatchList(); //页面加载
  },
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken()
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/foodprocess/flow/UpLoad";
  }
};
</script>
<style>

.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>